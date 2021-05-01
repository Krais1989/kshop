using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace KShop.Tracing
{
    public static class KShopTracingServiceExtensions
    {
        public static IServiceCollection AddKShopTracing(this IServiceCollection service, IConfiguration config)
        {
            service.AddOpenTelemetryTracing(tpb =>
            {
                tpb
                .AddAspNetCoreInstrumentation()
                .AddSource(KShopMassTransitTracingExtensions.KShopMassTransitSourceName)
                .AddJaegerExporter(o =>
                {
                    o.AgentHost = "localhost";
                    o.AgentPort = 6831;
                    o.MaxPayloadSizeInBytes = 4096;
                    o.ExportProcessorType = ExportProcessorType.Batch;
                    o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>()
                    {
                        MaxQueueSize = 2048,
                        ScheduledDelayMilliseconds = 5000,
                        ExporterTimeoutMilliseconds = 30000,
                        MaxExportBatchSize = 512
                    };
                });
            });

            return service;
        }
    }
}
