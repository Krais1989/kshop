using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KShop.Shared.Authentication
{
    public static class AuthServicesExtensions
    {
        public static uint? GetUserID(this ClaimsPrincipal principals)
        {
            var idRaw = principals.Claims.SingleOrDefault(c => c.Type == "id")?.Value ?? null;
            if (uint.TryParse(idRaw, out uint res))
                return res;
            else
                return null;
        }

        public static uint? GetCurrentUserID(this ControllerBase controller)
        {
            return GetUserID(controller.User);
        }

        public static uint GetCurrentUserIDExcept(this ControllerBase controller)
        {
            var result = controller.GetCurrentUserID();
            return result.Value;
        }


        public static void AddKShopAuth(this IServiceCollection services, IConfiguration config, string sectionName = "JwtSettings")
        {
            services.Configure<JwtSettings>(config.GetSection(sectionName));
            var jwtSettings = config.GetSection(sectionName).Get<JwtSettings>();

            /* Валидация JWT токена */
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtSettings.GetSymmetricSecurityKey(),

                    ValidateAudience = false,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = false,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateLifetime = true,
                };
            });
        }
    }
}
