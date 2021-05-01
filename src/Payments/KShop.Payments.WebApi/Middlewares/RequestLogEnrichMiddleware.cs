using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.WebApi.Middlewares
{
    public class RequestLogEnrichMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogEnrichMiddleware> _logger;

        public RequestLogEnrichMiddleware(
            RequestDelegate next,
            ILogger<RequestLogEnrichMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("CorrelationId"))
            {
                context.Request.Headers["CorrelationId"] = Guid.NewGuid().ToString();
            }

            var corId = context.Request.Headers["CorrelationId"];

            _logger.LogInformation("Enrich {CorrelationId}", corId);

            using (LogContext.PushProperty("CorrelationId", corId))
            {
                return _next.Invoke(context);
            }
        }
    }
}
