using FluentValidation.AspNetCore;
using KShop.Shared.Authentication;
using KShop.Shared.Integration.MassTransit;
using KShop.Shared.WebApi;
using KShop.Shipments.Domain;
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
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)), x => {
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
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

            services.AddKShopAuth(Configuration);

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
            app.UseKShopExceptionHandler();

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
