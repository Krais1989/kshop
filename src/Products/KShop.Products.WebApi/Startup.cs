using FluentValidation.AspNetCore;
using KShop.Communications.ServiceBus;
using KShop.Metrics;
using KShop.Products.Domain.ProductsReservation.BackgroundServices;
using KShop.Products.Domain.ProductsReservation.Consumers;
using KShop.Products.Domain.ProductsReservation.Mediators;
using KShop.Products.Domain.ProductsReservation.Validators;
using KShop.Products.Persistence;
using KShop.ServiceBus;
using KShop.Tracing;
using MassTransit;
using MassTransit.Definition;
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

namespace KShop.Products.WebApi
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
            services.AddDbContext<ProductsContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)), x =>
                {
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                });
            });

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                    busServices.AddConsumers(typeof(ProductsReservationSvcRequestConsumer).Assembly);
                },
                (busContext, rabbigConfig) =>
                {

                });

            services.AddKShopMetrics(Configuration);
            services.AddKShopTracing(Configuration);
            services.AddKShopSwagger(Configuration);
            services.AddMediatR(typeof(ProductsReserveMediatorHandler).Assembly);

            services.AddControllers()
                .AddMetrics()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ProductsReserveFluentValidator).Assembly));

            services.AddHostedService<ProductsReservationBackgroundService>();
        }

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
