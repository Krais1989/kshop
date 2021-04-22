using FluentValidation.AspNetCore;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.ServiceBus;
using KShop.Orders.Domain.Consumers;
using KShop.Orders.Domain.Handlers;
using KShop.Orders.Domain.RoutingSlips;
using KShop.Orders.Domain.Sagas;
using KShop.Orders.Domain.Validators;
using KShop.Orders.Persistence;
using KShop.ServiceBus;
using MassTransit;
using MassTransit.Conductor;
using MassTransit.Definition;
using MassTransit.Saga;
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

namespace KShop.Orders.WebApi
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

            services.AddDbContext<OrderContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                //db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));

                db.UseMySql(constr, x => { x.ServerVersion(new ServerVersion(new Version(8, 0))); });
            });

            var rabbinCon = new RabbitConnection();
            Configuration.GetSection("RabbitConnection").Bind(rabbinCon);

            services.AddMassTransit(busConfig =>
            {
                busConfig.ApplyKShopMassTransitConfiguration();
                //busConfig.SetKebabCaseEndpointNameFormatter();
                busConfig.AddRabbitMqMessageScheduler();

                busConfig.AddRequestClient<OrderGetStatus_SagaRequest>();
                busConfig.AddRequestClient<OrderCreate_SagaRequest>();

                //busConfig.AddActivities(typeof(OrderCreateCourierActivity).Assembly);
                //busConfig.AddConsumers(typeof(OrderCreate_RequestConsumer).Assembly);
                //busConfig.AddSagas(typeof(OrderSagaStateMachine).Assembly);

                busConfig
                    .AddSagaStateMachine<OrderSagaStateMachine, OrderSagaState>(typeof(OrderSagaStateMachineDefinition))
                    .InMemoryRepository();
                //.RedisRepository(Configuration.GetConnectionString("RedisConnection"), redisConfig =>
                //{
                //    redisConfig.ConcurrencyMode = MassTransit.RedisIntegration.ConcurrencyMode.Optimistic;
                //    redisConfig.KeyPrefix = "markettest";
                //});

                // busConfig.AddActivities(typeof(OrderCreateCourierActivity).Assembly);
                busConfig.AddConsumers(typeof(OrderCreate_RequestConsumer).Assembly);


                busConfig.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ApplyKShopBusConfiguration();

                    cfg.Host(rabbinCon.HostName, rabbinCon.Port, rabbinCon.VirtualHost, entryName, host =>
                    {
                        host.Username(rabbinCon.Username);
                        host.Password(rabbinCon.Password);
                    });

                    cfg.UseRabbitMqMessageScheduler();
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            services.AddMassTransitHostedService();


            services.AddMediatR(typeof(OrderCreateMediatorHandler).Assembly);

            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(OrderCreateFluentValidator).Assembly));

            services.AddMarketTestSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
