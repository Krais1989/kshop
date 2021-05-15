using FluentValidation.AspNetCore;
using KShop.Communications.ServiceBus;
using KShop.ServiceBus;
using KShop.Shipments.Domain.ExternalServices;
using KShop.Shipments.Domain.ShipmentProcessing.BackgroundServices;
using KShop.Shipments.Domain.ShipmentProcessing.Consumers;
using KShop.Shipments.Domain.ShipmentProcessing.Mediators;
using KShop.Shipments.Domain.ShipmentProcessing.Validators;
using KShop.Shipments.Persistence;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var entryName = Assembly.GetEntryAssembly().FullName;

            services.AddDbContext<ShipmentContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, x => { x.ServerVersion(new ServerVersion(new Version(8, 0))); });
            });

            var rabbinCon = new RabbitConnection();
            Configuration.GetSection("RabbitConnection").Bind(rabbinCon);

            services.AddMassTransit(busConfig =>
            {
                busConfig.ApplyKShopMassTransitConfiguration();
                busConfig.AddDelayedMessageScheduler();

                //busConfig.AddRequestClient<OrderGetStatusSagaRequest>();
                //busConfig.AddRequestClient<OrderPlacingSagaRequest>();

                busConfig.AddConsumers(typeof(ShipmentCreateSvcConsumer).Assembly);


                busConfig.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ApplyKShopBusConfiguration();

                    cfg.Host(rabbinCon.HostName, rabbinCon.Port, rabbinCon.VirtualHost, entryName, host =>
                    {
                        host.Username(rabbinCon.Username);
                        host.Password(rabbinCon.Password);
                    });

                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddMassTransitHostedService();

            services.AddMediatR(typeof(ShipmentCreateMediatorHandler).Assembly);

            services.AddMarketTestSwagger(Configuration);

            services.AddHostedService<ShipmentInitBackgroundService>();
            services.AddHostedService<ShipmentCheckBackgroundService>();
            services.AddHostedService<ShipmentCancellingBackgroundService>();

            services.AddSingleton<IExternalShipmentService, MockExternallShipmentService>();

            services.AddControllers()
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
