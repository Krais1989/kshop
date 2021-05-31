using FluentValidation.AspNetCore;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Products;
using KShop.Communications.ServiceBus;
using KShop.Metrics;
using KShop.Orders.Domain.Consumers;
using KShop.Orders.Domain.Handlers;
using KShop.Orders.Domain.Orchestrations;
using KShop.Orders.Domain.RoutingSlips.OrderInitialization;
using KShop.Orders.Domain.Validators;
using KShop.Orders.Persistence;
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

namespace KShop.Orders.WebApi
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
            services.AddStackExchangeRedisCache(r => { 
                r.Configuration = Configuration.GetConnectionString("RedisConnection");
            });

            services.AddDbContext<OrderContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, x => { 
                    x.ServerVersion(new ServerVersion(new Version(8, 0)));
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                });
            });

            services.AddMediatR(typeof(OrderCreateMediatorHandler).Assembly);
            services.AddKShopTracing(Configuration);
            services.AddKShopMetrics(Configuration);
            services.AddKShopSwagger(Configuration);

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                    busServices.AddRequestClient<OrderGetStatusSagaRequest>();
                    busServices.AddRequestClient<OrderPlacingSagaRequest>();
                    busServices.AddRequestClient<OrderCreateSvcRequest>();

                    busServices.AddRequestClient<OrderSetStatusCancelledSvcRequest>();
                    busServices.AddRequestClient<OrderSetStatusFaultedSvcRequest>();
                    busServices.AddRequestClient<OrderSetStatusPayedSvcRequest>();
                    busServices.AddRequestClient<OrderSetStatusRefundedSvcRequest>();
                    busServices.AddRequestClient<OrderSetStatusReservedSvcRequest>();
                    busServices.AddRequestClient<OrderSetStatusShippedSvcRequest>();

                    busServices.AddRequestClient<ProductsReserveSvcRequest>();
                    busServices.AddRequestClient<ProductsReserveCancelSvcRequest>();


                    busServices.AddActivities(typeof(OrderCreateRSActivity).Assembly);

                    busServices.AddConsumers(typeof(OrderCreateSvcRequestConsumer).Assembly);
                    busServices
                        .AddSagaStateMachine<OrderProcessingSagaStateMachine, OrderProcessingSagaState>(typeof(OrderProcessingSagaStateMachineDefinition))
                        .AddKShopSagaRedisStorage(Configuration);
                        //.InMemoryRepository();
                },
                (busContext, rabbigConfig) =>
                {
                });

            services.AddControllers()
                .AddMetrics()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(OrderCreateFluentValidator).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{EntryAssemblyName} v1");
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
