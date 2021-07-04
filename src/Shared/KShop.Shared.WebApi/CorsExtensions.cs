using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Shared.WebApi
{
    public static class CorsExtensions
    {
        public static IApplicationBuilder UseKShopCors(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseCors(e =>
            {
                e.AllowAnyMethod();
                e.AllowAnyHeader();
                e.AllowAnyOrigin();
            });
            return app;
        }
    }
}
