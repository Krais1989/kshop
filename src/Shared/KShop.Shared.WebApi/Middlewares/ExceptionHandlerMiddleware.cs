using KShop.Shared.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Shared.WebApi
{

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseKShopExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }


    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseBadRequestException e)
            {
                context.Response.StatusCode = e.StatusCode;
                var txt = JsonSerializer.Serialize(new { e.StatusCode, e.Message });
                await context.Response.WriteAsync(txt);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}
