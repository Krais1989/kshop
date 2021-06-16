using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace KShop.Shared.WebApi
{
    public class TracingOptions
    {
        public string[] Sources { get; set; }
        public JaegerExporterOptions JaegerOptions { get; set; }
        public ConsoleExporterOptions ConsoleOptions { get; set; }
    }

    public class ConsoleExporterOptions
    {
        public ConsoleExporterOutputTargets Targets { get; set; }
    }

    public class JaegerExporterOptions
    {
        public class CBatchExportProcessorOptions
        {
            public int MaxQueueSize { get; set; }
            public int ScheduledDelayMilliseconds { get; set; }
            public int ExporterTimeoutMilliseconds { get; set; }
            public int MaxExportBatchSize { get; set; }
        }

        public string AgentHost { get; set; }
        public int AgentPort { get; set; }
        public int? MaxPayloadSizeInBytes { get; set; }
        public ExportProcessorType ExportProcessorType { get; set; }
        public CBatchExportProcessorOptions BatchExportProcessorOptions { get; set; }
    }

    public static class TracingServiceExtensions
    {
        public static IServiceCollection AddKShopTracing(
            this IServiceCollection service,
            IConfiguration config,
            Action<TracerProviderBuilder> builderCallback = null)
        {
            var trace_options = new TracingOptions();
            config.GetSection("TracingOptions").Bind(trace_options);

            service.AddOpenTelemetryTracing(tpb =>
            {
                tpb.AddAspNetCoreInstrumentation();

                if (trace_options.Sources != null)
                {
                    foreach (var srcs in trace_options.Sources)
                    {
                        tpb.AddSource(srcs);
                    }
                }

                if (trace_options.JaegerOptions != null)
                {
                    var jaeger_opetions = trace_options.JaegerOptions;
                    tpb.AddJaegerExporter(o =>
                     {
                         o.AgentHost = jaeger_opetions.AgentHost;
                         o.AgentPort = jaeger_opetions.AgentPort;
                         o.MaxPayloadSizeInBytes = jaeger_opetions.MaxPayloadSizeInBytes;
                         o.ExportProcessorType = jaeger_opetions.ExportProcessorType;

                         if (jaeger_opetions.BatchExportProcessorOptions != null)
                         {
                             var batch_options = jaeger_opetions.BatchExportProcessorOptions;
                             o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>()
                             {
                                 MaxQueueSize = batch_options.MaxQueueSize,
                                 ScheduledDelayMilliseconds = batch_options.ScheduledDelayMilliseconds,
                                 ExporterTimeoutMilliseconds = batch_options.ExporterTimeoutMilliseconds,
                                 MaxExportBatchSize = batch_options.MaxExportBatchSize
                             };
                         }
                     });
                }

                if (trace_options.ConsoleOptions != null)
                {
                    tpb.AddConsoleExporter(o =>
                    {
                        o.Targets = trace_options.ConsoleOptions.Targets;
                    });
                }

                if (builderCallback != null)
                    builderCallback(tpb);
            });

            return service;
        }
    }
}
