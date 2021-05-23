
using Automatonymous.Requests;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.OrderPlacing.Activities;
using KShop.Orders.Domain.RoutingSlips;
using KShop.Orders.Domain.RoutingSlips.OrderInitialization;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Events;
using MassTransit.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Consumers
{


    /// <summary>
    /// 
    /// </summary>
    public class OrderPlacingRSConsumer : IConsumer<OrderPlacingRSRequest>
    {
        private readonly ILogger<OrderPlacingRSConsumer> _logger;
        private readonly IEndpointNameFormatter _formatter;

        public OrderPlacingRSConsumer(
            ILogger<OrderPlacingRSConsumer> logger,
            IEndpointNameFormatter formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        public async Task Consume(ConsumeContext<OrderPlacingRSRequest> context)
        {
            var msgData = JsonSerializer.Serialize(context.Message);
            _logger.LogInformation($"{nameof(OrderPlacingRSConsumer)} consumes: {msgData}");

            var rs = CreateRoutingSlip(context);
            await context.Execute(rs);

            if (context.ResponseAddress != null)
            {

            }
        }

        private RoutingSlip CreateRoutingSlip(ConsumeContext<OrderPlacingRSRequest> context)
        {
            var createActivity = _formatter.ExecuteActivity<OrderCreateRSActivity, OrderCreateRSActivityArgs>();
            var reserveActivity = _formatter.ExecuteActivity<ProductsReserveRSActivity, ProductsReserveRSActivityArgs>();

            var builder = new RoutingSlipBuilder(Guid.NewGuid());


            /**/
            builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Completed,
                RoutingSlipEventContents.All,
                x => x.Send(new OrderPlacingCompletedRSEvent
                {
                    SubmissionID = context.Message.SubmissionID
                }));

            builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Faulted,
                RoutingSlipEventContents.All,
                x => x.Send(new OrderPlacingFaultedRSEvent
                {
                    SubmissionID = context.Message.SubmissionID
                }));


            builder.AddVariable("OrderID", context.Message.SubmissionID); // общая переменная routing slip
            builder.AddVariable("CustomerID", context.Message.CustomerID); // общая переменная routing slip
            builder.AddVariable("Positions", context.Message.Positions); // общая переменная routing slip
            builder.AddVariable(nameof(ConsumeContext.RequestId), context.RequestId);
            builder.AddVariable(nameof(ConsumeContext.ResponseAddress), context.ResponseAddress);
            builder.AddVariable("Request", context.Message);


            builder.AddActivity(nameof(OrderCreateRSActivity), new Uri($"exchange:{createActivity}"),
                new OrderCreateRSActivityArgs
                {
                });

            builder.AddActivity(nameof(ProductsReserveRSActivity), new Uri($"exchange:{reserveActivity}"),
                new ProductsReserveRSActivityArgs
                {
                });
            return builder.Build();
        }

        //public async Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        //{
        //    var orderId = context.Message.GetVariable<Guid>("OrderID");
        //    var requestId = context.Message.GetVariable<Guid?>(nameof(ConsumeContext.RequestId));
        //    var responseAddress = context.Message.GetVariable<Uri>(nameof(ConsumeContext.ResponseAddress));

        //    if (requestId.HasValue && responseAddress != null)
        //    {
        //        var responseEndpoint = await context.GetResponseEndpoint<OrderPlacingSuccessRSEvent>(responseAddress);
        //        await responseEndpoint.Send(
        //            new OrderPlacingSuccessRSEvent
        //            {
        //                SuccessMessage = "SUCCESS"
        //            });
        //    }
        //}

        //public async Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        //{
        //    var requestId = context.Message.GetVariable<Guid?>(nameof(ConsumeContext.RequestId));
        //    var responseAddress = context.Message.GetVariable<Uri>(nameof(ConsumeContext.ResponseAddress));
        //    var request = context.Message.GetVariable<OrderPlacingRSRequest>("Request");

        //    if (requestId.HasValue && responseAddress != null)
        //    {
        //        var responseEndpoint = await context.GetResponseEndpoint<OrderPlacingSuccessRSEvent>(responseAddress);
        //        IEnumerable<ExceptionInfo> exceptions = context.Message.ActivityExceptions.Select(x => x.ExceptionInfo);
        //        Fault<OrderPlacingRSRequest> response =
        //            new FaultEvent<OrderPlacingRSRequest>(request, requestId, context.Host, exceptions, TypeMetadataCache<OrderPlacingRSRequest>.MessageTypeNames);
        //        await responseEndpoint.Send(response);
        //    }
        //}


    }


}
