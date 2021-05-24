using FluentValidation.AspNetCore;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.ServiceBus;
using KShop.Metrics;
using KShop.Payments.Domain.BackgroundServices;
using KShop.Payments.Domain.Consumers;
using KShop.Payments.Domain.ExternalPaymentProviders.Common;
using KShop.Payments.Domain.ExternalPaymentProviders.Mocking;
using KShop.Payments.Domain.Mediators;
using KShop.Payments.Domain.Validators;
using KShop.Payments.Persistence;
using KShop.ServiceBus;
using KShop.Tracing;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Swagger;
using System;
using System.Reflection;

namespace KShop.Payments.WebApi
{
    public class Startup
    {
        private string EntryAssemblyName => Assembly.GetEntryAssembly().FullName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PaymentsContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                //db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
                db.UseMySql(constr, x => { 
                    x.ServerVersion(new ServerVersion(new Version(8, 0)));
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
                });
            });

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                    busServices.AddConsumers(typeof(PaymentCreateSvcConsumer).Assembly);
                    busServices.AddRequestClient<PaymentCreateSvcCommand>();
                    busServices.AddRequestClient<PaymentCancelSvcRequest>();
                },
                (busContext, rabbigConfig) =>
                {

                });

            services.AddKShopMetrics(Configuration);
            services.AddKShopTracing(Configuration);
            services.AddKShopSwagger(Configuration);
            services.AddMediatR(typeof(PaymentInitializeMediatorHandler).Assembly);

            services.AddControllers()
                .AddMetrics()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(PaymentCreatedFluentValidator).Assembly));

            services.AddHostedService<PaymentInitializingBackgroundService>();
            services.AddHostedService<PaymentCheckingBackgroundService>();
            services.AddHostedService<PaymentCancellingBackgroundService>();
            services.AddSingleton<ICommonPaymentProvider, CommonPaymentProvider>();
            services.AddSingleton<IMockExternalPaymentProvider, MockExternalPaymentProvider>();
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
