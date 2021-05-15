using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.InfluxDB;
using FluentValidation.AspNetCore;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.ServiceBus;
using KShop.Payments.Domain.BackgroundServices;
using KShop.Payments.Domain.Consumers;
using KShop.Payments.Domain.Mediators;
using KShop.Payments.Domain.Validators;
using KShop.Payments.Persistence;
using KShop.ServiceBus;
using KShop.Tracing;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Serilog;
using Swagger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KShop.Payments.WebApi
{
    public class Startup
    {
        private string entryName = Assembly.GetEntryAssembly().FullName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureBackgroudServices(IServiceCollection services)
        {
            services.AddHostedService<PaymentInitBackgroundService>();
            services.AddHostedService<PaymentCheckBackgroundService>();
            services.AddHostedService<PaymentCancellingBackgroundService>();
            //services.AddHostedService<TelemetrySenderBackgroundService>();
            //services.AddHostedService<TestAlgorithmsBackgroundService>();
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<PaymentsContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                //db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
                db.UseMySql(constr, x => { x.ServerVersion(new ServerVersion(new Version(8, 0))); });
            });
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbinCon = new RabbitConnection();
            Configuration.GetSection("RabbitConnection").Bind(rabbinCon);

            services.AddMassTransit(x =>
            {
                x.ApplyKShopMassTransitConfiguration();
                x.AddConsumers(typeof(PaymentCreateConsumer).Assembly);

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbinCon.HostName, rabbinCon.Port, rabbinCon.VirtualHost, entryName, host =>
                    {
                        host.Username(rabbinCon.Username);
                        host.Password(rabbinCon.Password);
                    });

                    //var endpointNameFormatter = ctx.GetRequiredService<IEndpointNameFormatter>();
                    //EndpointConvention.Map<OrderSagaStateMachine>(new Uri($"queue:{endpointNameFormatter.Consumer<>}"))

                    cfg.KShopTraceConsumeFilter(ctx);
                    cfg.KShopTracePublishFilter(ctx);
                    cfg.KShopTraceSendFilter(ctx);

                    /* ��������! ������������� ������������� ��������� ��� ���� ������������������ ����������/���/�������� ��������� ��������� ��������� */
                    cfg.ConfigureEndpoints(ctx);
                });

                x.AddRequestClient<PaymentCreateSvcRequest>();
                x.AddRequestClient<PaymentCancelSvcRequest>();
            }).AddMassTransitHostedService();
        }

        private void ConfigureTracing(IServiceCollection services)
        {
            //var traceProvider = Sdk.CreateTracerProviderBuilder()
            //    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("dotnet-jaeger-test"))
            //    .AddSource("MassTransit", "KShopTestTrace")
            //    .AddJaegerExporter(o =>
            //    {
            //        o.AgentHost = "localhost";
            //        o.AgentPort = 6831;
            //        o.MaxPayloadSizeInBytes = 4096;
            //        o.ExportProcessorType = ExportProcessorType.Batch;
            //        o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>()
            //        {
            //            MaxQueueSize = 2048,
            //            ScheduledDelayMilliseconds = 5000,
            //            ExporterTimeoutMilliseconds = 30000,
            //            MaxExportBatchSize = 512
            //        };
            //    })
            //    .Build();

            services.AddKShopTracing(Configuration);
        }

        private void ConfigureMetrics(IServiceCollection services)
        {
            //var filter = new MetricsFilter().WhereType(MetricType.Counter| MetricType.Timer);
            var builder = AppMetrics.CreateDefaultBuilder();
            //builder.Configuration.ReadFrom(Configuration);

            var metrics = builder
                .OutputMetrics.AsInfluxDbLineProtocol()
                .OutputEnvInfo.AsPlainText()
                .Configuration.Configure(opt =>
                {
                    opt.DefaultContextLabel = "KShop";
                    opt.Enabled = true;
                    opt.ReportingEnabled = true;
                    opt.GlobalTags.Add("mytag", "mytagvalue");
                })
                .Report.ToInfluxDb(influx =>
                {
                    influx.InfluxDb.BaseUri = new Uri("http://localhost:8086");
                    influx.InfluxDb.Database = "db_kshop_metrics";
                    //influx.InfluxDb.Consistenency = "consistency";
                    influx.InfluxDb.UserName = "influx";
                    influx.InfluxDb.Password = "asdasdasd";
                    //influx.InfluxDb.RetentionPolicy = "rp";
                    influx.InfluxDb.CreateDataBaseIfNotExists = true;

                    influx.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    influx.HttpPolicy.FailuresBeforeBackoff = 5;
                    influx.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);

                    influx.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();

                    //influx.Filter = filter;

                    influx.FlushInterval = TimeSpan.FromSeconds(5);
                })
                .Build();

            services.AddAppMetricsHealthPublishing();
            services.AddAppMetricsGcEventsMetricsCollector();
            services.AddAppMetricsSystemMetricsCollector();

            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints(opt =>
            {
                // site/metrics
                opt.MetricsEndpointEnabled = true;
                opt.MetricsEndpointOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();

                // site/metrics-text
                opt.MetricsTextEndpointEnabled = true;
                opt.MetricsTextEndpointOutputFormatter = new MetricsTextOutputFormatter();

                // site/env
                opt.EnvironmentInfoEndpointEnabled = true;
                opt.EnvInfoEndpointOutputFormatter = new EnvInfoTextOutputFormatter();
            });

            services.AddMetricsReportingHostedService();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDbContext(services);
            ConfigureBackgroudServices(services);
            ConfigureMassTransit(services);
            ConfigureMetrics(services);
            ConfigureTracing(services);

            services.AddMediatR(typeof(PaymentCreateMediatorHandler).Assembly);

            services.AddControllers()
                .AddMetrics()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(PaymentCreatedFluentValidator).Assembly));

            services.AddMarketTestSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricsAllMiddleware();
            // Or to cherry-pick the tracking of interest
            // app.UseMetricsActiveRequestMiddleware();
            // app.UseMetricsErrorTrackingMiddleware();
            // app.UseMetricsPostAndPutSizeTrackingMiddleware();
            // app.UseMetricsRequestTrackingMiddleware();
            // app.UseMetricsOAuth2TrackingMiddleware();
            // app.UseMetricsApdexTrackingMiddleware();

            app.UseMetricsAllEndpoints();
            // Or to cherry-pick endpoint of interest
            // app.UseMetricsEndpoint();
            // app.UseMetricsTextEndpoint();
            // app.UseEnvInfoEndpoint();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
