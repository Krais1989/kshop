using Automatonymous.Requests;
using KShop.Shared.Integration.Contracts;
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


namespace KShop.Orders.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderPlacingRSConsumer : IConsumer<OrderPlacingRSRequest>
    {
        private readonly ILogger<OrderPlacingRSConsumer> _logger;
        private readonly IEndpointNameFormatter _formatter;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderPlacingRSConsumer(
            ILogger<OrderPlacingRSConsumer> logger,
            IEndpointNameFormatter formatter,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _formatter = formatter;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderPlacingRSRequest> context)
        {
            try
            {
                var msgData = JsonSerializer.Serialize(context.Message);
                _logger.LogInformation($"{nameof(OrderPlacingRSConsumer)} consumes: {msgData}");

                var rs = CreateRoutingSlip(context);
                await context.Execute(rs);
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish(new ProductsReserveFaultEvent(context.Message.OrderID, e.Message));
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
                x => x.Send(new OrderReservingCompletedRSEvent
                {
                    OrderID = context.Message.OrderID
                }));

            builder.AddSubscription(context.SourceAddress,
                RoutingSlipEvents.Faulted,
                RoutingSlipEventContents.All,
                x => x.Send(new ProductsReserveFaultEvent(context.Message.OrderID)));


            builder.AddVariable("OrderID", context.Message.OrderID); // общая переменная routing slip
            builder.AddVariable("CustomerID", context.Message.CustomerID); // общая переменная routing slip
            builder.AddVariable("OrderContent", context.Message.OrderContent); // общая переменная routing slip
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
