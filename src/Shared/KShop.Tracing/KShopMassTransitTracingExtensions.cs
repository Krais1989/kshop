using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using GreenPipes;
using MassTransit.SendPipeSpecifications;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace KShop.Tracing
{
    public class TracingSendFilter<T> : IFilter<SendContext<T>>
        where T : class
    {
        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            var name = context.ResponseAddress == null 
                ? $"Send: {context.DestinationAddress.LocalPath}"
                : $"Respond: {context.ResponseAddress.LocalPath}";


            using var activity = KShopMassTransitTracingExtensions.KShopMassTransitSource.StartActivity(name);

            if (!context.Headers.TryGetHeader("trace_id", out object existTraceIdString))
            {
                context.Headers.Set("trace_id", activity.TraceId.ToHexString());
            }

            if (!context.Headers.TryGetHeader("parent_span_id", out object existParentSpanIdString))
            {
                context.Headers.Set("parent_span_id", activity.ParentSpanId.ToHexString());
            }
            await next.Send(context);
        }
    }

    public class TracingPublishFilter<T> : IFilter<PublishContext<T>>
        where T : class
    {
        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
        {
            var name = $"Publish: {context.DestinationAddress.LocalPath}";
            using var activity = KShopMassTransitTracingExtensions.KShopMassTransitSource.StartActivity(name);

            if (!context.Headers.TryGetHeader("trace_id", out object existTraceId))
            {
                context.Headers.Set("trace_id", activity.TraceId.ToHexString());
            }

            if (!context.Headers.TryGetHeader("parent_span_id", out object existParentSpanId))
            {
                context.Headers.Set("parent_span_id", activity.ParentSpanId.ToHexString());
            }

            await next.Send(context);
        }
    }

    public class TracingConsumeFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        public void Probe(ProbeContext context)
        {

        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var traceIdString = context.Headers.Get<string>("trace_id", null);
            var parentSpanIdString = context.Headers.Get<string>("parent_span_id", null);

            var traceId = ActivityTraceId.CreateFromString(traceIdString);
            var parentSpanId = ActivitySpanId.CreateFromString(parentSpanIdString);

            var ac = new ActivityContext(traceId, parentSpanId, ActivityTraceFlags.Recorded, null, true);

            var name = $"Consume: {context.ReceiveContext.InputAddress.LocalPath}";
            using var activity
                = KShopMassTransitTracingExtensions.KShopMassTransitSource.StartActivity(name, ActivityKind.Consumer, ac);
            await next.Send(context);
        }
    }

    public static class KShopMassTransitTracingExtensions
    {
        public const string KShopMassTransitSourceName = "KShopMassTransit";
        public static readonly ActivitySource KShopMassTransitSource = new ActivitySource(KShopMassTransitSourceName);

        public static void KShopTraceConsumeFilter(this IConsumePipeConfigurator conf, IBusRegistrationContext ctx)
        {
            conf.UseConsumeFilter(typeof(TracingConsumeFilter<>), ctx);
        }

        public static void KShopTraceSendFilter(this ISendPipelineConfigurator conf, IBusRegistrationContext ctx)
        {
            conf.UseSendFilter(typeof(TracingSendFilter<>), ctx);
        }

        public static void KShopTracePublishFilter(this IPublishPipelineConfigurator conf, IBusRegistrationContext ctx)
        {
            conf.UsePublishFilter(typeof(TracingPublishFilter<>), ctx);
        }
    }
}
