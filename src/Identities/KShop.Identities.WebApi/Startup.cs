using FluentValidation.AspNetCore;
using KShop.Auth;
using KShop.Communications.ServiceBus;
using KShop.Identities.Domain;
using KShop.Identities.Domain.SignIn.Mediators;
using KShop.Identities.Persistence;
using KShop.Metrics;
using KShop.Tracing;
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
using Serilog;
using Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KShop.Identities.WebApi
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

            services.AddDbContext<IdentityContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)), x => {
                    x.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null);
                });
            });

            services.AddMediatR(typeof(EmailPasswordSignInHandler).Assembly);
            services.AddKShopTracing(Configuration);
            services.AddKShopMetrics(Configuration);
            services.AddKShopSwagger(Configuration);

            services.AddKShopAuth(Configuration);
            services.AddKNewsIdentity(Configuration);

            services.AddKShopMassTransitRabbitMq(Configuration,
                busServices =>
                {
                    
                },
                (busContext, rabbigConfig) =>
                {
                });

            services.AddSingleton<IJWTFactory, JwtFactory>();

            services.AddControllers()
                .AddMetrics();
                //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(OrderCreateFluentValidator).Assembly));
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
