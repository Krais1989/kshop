using FluentValidation.AspNetCore;
using KShop.Communications.ServiceBus;
using KShop.Metrics;
using KShop.ServiceBus;
using KShop.Shipments.Domain.BackgroundServices;
using KShop.Shipments.Domain.Consumers;
using KShop.Shipments.Domain.ExternalShipmentProviders;
using KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions;
using KShop.Shipments.Domain.ExternalShipmentProviders.Mocking;
using KShop.Shipments.Domain.Mediators;
using KShop.Shipments.Domain.Validators;
using KShop.Shipments.Persistence;
using KShop.Tracing;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KShop.Shipments.WebApi
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
            services.AddDbContext<ShipmentContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, x => {
                    x.ServerVersion(new ServerVersion(new Version(8, 0)));
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
                });
            });

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                    busServices.AddConsumers(typeof(ShipmentCreateSvcConsumer).Assembly);
                },
                (busContext, rabbigConfig) =>
                {

                });

            services.AddKShopMetrics(Configuration);
            services.AddKShopTracing(Configuration);
            services.AddKShopSwagger(Configuration);

            services.AddMediatR(typeof(ShipmentInitializeMediatorHandler).Assembly);

            services.AddHostedService<ShipmentInitializingBackgroundService>();
            services.AddHostedService<ShipmentCheckBackgroundService>();
            services.AddHostedService<ShipmentCancellingBackgroundService>();

            services.AddSingleton<IExternalShipmentServiceProvider, MockExternallShipmentProvider>();

            services.AddControllers()
                .AddMetrics()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ShipmentCreateFluentValidator).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KShop.Shipments.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
