using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Swagger
{
    public static class SwaggerServicesExtensions
    {
        private static string ExecutingAssemblyName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Интеграция Swagger
        /// Загружает OpenApiInfo из конфига SwaggerSettings 
        /// </summary>
        public static void AddMarketTestSwagger(this IServiceCollection services, IConfiguration config, string sectionName = "SwaggerSettings")
        {
            var openApiInfo = config.GetSection(sectionName)?.Get<OpenApiInfo>();

            if (openApiInfo == null)
            {
                openApiInfo = new OpenApiInfo()
                {
                    Title = $"{ExecutingAssemblyName}",
                    Version = "v1",
                    Description = $"Swagger UI for {ExecutingAssemblyName}",
                    Contact = new OpenApiContact()
                    {
                        Name = "Egor Krais",
                        Email = "krais1989@gmail.com"
                    }
                };
            }

            var openApiSecurityScheme = new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(openApiInfo.Version, openApiInfo);
                c.AddSecurityDefinition("BearerAuth", openApiSecurityScheme);
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" }
                            },
                            new string[] { }
                        }
                    });

                var xmlFile = $"{ExecutingAssemblyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
