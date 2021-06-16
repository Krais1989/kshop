using FluentValidation.AspNetCore;
using KShop.Products.Domain;
using KShop.Products.Persistence;
using KShop.Shared.Authentication;
using KShop.Shared.Integration.MassTransit;
using KShop.Shared.WebApi;
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

            services.AddKShopAuth(Configuration);

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
