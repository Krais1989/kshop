using FluentValidation.AspNetCore;
using KShop.Carts.Domain.Mediators;
using KShop.Carts.Persistence;
using KShop.Shared.Authentication;
using KShop.Shared.Integration.MassTransit;
using KShop.Shared.WebApi;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi
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

            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<MongoSettings>(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddScoped<ICartKVRepository, MongoCartRepository>();

            //services.AddKShopTracing(Configuration);
            //services.AddKShopMetrics(Configuration);
            services.AddKShopSwagger(Configuration);

            services.AddKShopAuth(Configuration);

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                },
                (busContext, rabbigConfig) =>
                {
                });

            services.AddMediatR(typeof(GetCurrentCartMediatorHandler).Assembly);

            services.AddControllers()
                //.AddMetrics();
                //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(OrderCreateFluentValidator).Assembly));
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseKShopExceptionHandler();

            //app.UseMetricsAllMiddleware();
            //app.UseMetricsAllEndpoints();

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
            app.UseKShopCors(Configuration);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
