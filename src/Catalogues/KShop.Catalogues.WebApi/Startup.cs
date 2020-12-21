using KShop.Catalogues.Domain.Consumers;
using KShop.Catalogues.Persistence;
using KShop.ServiceBus;
using MassTransit;
using MassTransit.Definition;
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
using Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KShop.Catalogues.WebApi
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

            services.AddDbContext<CatalogueContext>(db =>
            {
                var constr = Configuration.GetConnectionString("DefaultConnection");
                db.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            });

            var rabbinCon = new RabbitConnection();
            Configuration.GetSection("RabbitConnection").Bind(rabbinCon);

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(typeof(OrderReservationConsumer).Assembly);
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbinCon.HostName, rabbinCon.Port, rabbinCon.VirtualHost, entryName, host =>
                    {
                        host.Username(rabbinCon.Username);
                        host.Password(rabbinCon.Password);
                    });

                    /* ��������! ������������� ������������� ��������� ��� ���� ������������������ ����������/���/�������� ��������� ��������� ��������� */
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
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
