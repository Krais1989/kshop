using FluentValidation.AspNetCore;
using KShop.Orders.Domain;
using KShop.Orders.Persistence;
using KShop.Shared.Authentication;
using KShop.Shared.Integration.Contracts;
using KShop.Shared.Integration.MassTransit;
using KShop.Shared.WebApi;
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
using Serilog;
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
            var config = (Configuration as IConfigurationRoot).GetDebugView();
            Log.Warning(config);

            services.AddStackExchangeRedisCache(r => { 
                r.Configuration = Configuration.GetConnectionString("RedisConnection");
            });

            services.AddDbContext<OrderContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)), x => {
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                });
            });

            services.AddMediatR(typeof(OrderCreateMediatorHandler).Assembly);
            services.AddKShopTracing(Configuration);
            services.AddKShopMetrics(Configuration);
            services.AddKShopSwagger(Configuration);

            services.AddKShopAuth(Configuration);

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
            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{EntryAssemblyName} v1");
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
