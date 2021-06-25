using FluentValidation.AspNetCore;
using KShop.Shared.Domain.Contracts;
using KShop.Payments.Persistence;
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
using System;
using System.Reflection;
using KShop.Shared.WebApi;
using KShop.Payments.Domain;
using KShop.Shared.Integration.MassTransit;
using KShop.Shared.Authentication;
using KShop.Shared.Integration.Contracts;

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
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)), x => {
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
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

            services.AddKShopAuth(Configuration);

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
            app.UseKShopExceptionHandler();

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
            app.AddKShopCors(Configuration);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
