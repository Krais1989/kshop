
//using Automatonymous.Requests;
//using KShop.Orders.Domain.RoutingSlips;
//using MassTransit;
//using MassTransit.Courier;
//using MassTransit.Courier.Contracts;
//using MassTransit.Events;
//using MassTransit.Metadata;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace KShop.Orders.Domain.Consumers
//{

//    public class CreateOrder_RoutingSlipRequest { 
    
//        public int CustomerID { get; set; }
//        public IDictionary<int,int> Positions { get; set; }
//    }
//    public class CreateOrder_RoutingSlipResponse
//    {
//        public string Data { get; set; }
//    }
//    public class CreateOrderSuccess_RoutingSlipMessage
//    {
//        public string SuccessMessage { get; set; }
//    }
//    public class CreateOrderFailure_RoutingSlipMessage
//    {
//        public string ErrorMessage { get; set; }
//    }

//    public class CreateOrderRoutingSlipConsumer :
//        IConsumer<CreateOrder_RoutingSlipRequest>,
//        IConsumer<RoutingSlipCompleted>,
//        IConsumer<RoutingSlipFaulted>
//    {
//        private readonly ILogger<CreateOrderRoutingSlipConsumer> _logger;
//        private readonly IEndpointNameFormatter _formatter;

//        public CreateOrderRoutingSlipConsumer(
//            ILogger<CreateOrderRoutingSlipConsumer> logger,
//            IEndpointNameFormatter formatter)
//        {
//            _logger = logger;
//            _formatter = formatter;
//        }

//        public async Task Consume(ConsumeContext<CreateOrder_RoutingSlipRequest> context)
//        {
//            var msgData = JsonSerializer.Serialize(context.Message);
//            _logger.LogInformation($"{nameof(CreateOrderRoutingSlipConsumer)} consumes: {msgData}");

//            var rs = CreateRoutingSlip(context);
//            await context.Execute(rs);

//            if (context.ResponseAddress != null)
//            {

//            }
//        }

//        private RoutingSlip CreateRoutingSlip(ConsumeContext<CreateOrder_RoutingSlipRequest> context)
//        {
//            var q1Name = _formatter.ExecuteActivity<OrderCreateCourierActivity, OrderCreateCourierArguments>();
//            var q2Name = _formatter.ExecuteActivity<InventoryReserveCourierActivity, InventoryReserveCourierArguments>();
//            var q3Name = _formatter.ExecuteActivity<InvoiceCreateCourierActivity, InvoiceCreateCourierArguments>();

//            var builder = new RoutingSlipBuilder(Guid.NewGuid());

//            builder.AddSubscription(context.ReceiveContext.InputAddress, RoutingSlipEvents.Completed | RoutingSlipEvents.Faulted);

//            builder.AddVariable("OrderID", Guid.Empty); // общая переменная routing slip
//            builder.AddVariable("CustomerID", context.Message.CustomerID); // общая переменная routing slip
//            builder.AddVariable("Positions", context.Message.Positions); // общая переменная routing slip
//            builder.AddVariable(nameof(ConsumeContext.RequestId), context.RequestId);
//            builder.AddVariable(nameof(ConsumeContext.ResponseAddress), context.ResponseAddress);
//            builder.AddVariable("Request", context.Message);

//            builder.AddActivity("OrderCreateCourierActivity", new Uri($"queue:{q1Name}"),
//                new OrderCreateCourierArguments
//                {
//                });

//            builder.AddActivity("InventoryReserveCourierActivity", new Uri($"queue:{q2Name}"),
//                new InventoryReserveCourierArguments
//                {
//                });

//            builder.AddActivity("InvoiceCreateCourierActivity", new Uri($"queue:{q3Name}"),
//                new InvoiceCreateCourierArguments
//                {
//                });
//            return builder.Build();
//        }

//        public async Task Consume(ConsumeContext<RoutingSlipCompleted> context)
//        {
//            var orderId = context.Message.GetVariable<Guid>("OrderID");
//            var requestId = context.Message.GetVariable<Guid?>(nameof(ConsumeContext.RequestId));
//            var responseAddress = context.Message.GetVariable<Uri>(nameof(ConsumeContext.ResponseAddress));

//            if (requestId.HasValue && responseAddress != null)
//            {
//                var responseEndpoint = await context.GetResponseEndpoint<CreateOrderSuccess_RoutingSlipMessage>(responseAddress);
//                await responseEndpoint.Send(
//                    new CreateOrderSuccess_RoutingSlipMessage
//                    {
//                        SuccessMessage = "SUCCESS"
//                    });
//            }
//        }

//        public async Task Consume(ConsumeContext<RoutingSlipFaulted> context)
//        {
//            var requestId = context.Message.GetVariable<Guid?>(nameof(ConsumeContext.RequestId));
//            var responseAddress = context.Message.GetVariable<Uri>(nameof(ConsumeContext.ResponseAddress));
//            var request = context.Message.GetVariable<CreateOrder_RoutingSlipRequest>("Request");

//            if (requestId.HasValue && responseAddress != null)
//            {
//                var responseEndpoint = await context.GetResponseEndpoint<CreateOrderSuccess_RoutingSlipMessage>(responseAddress);
//                IEnumerable<ExceptionInfo> exceptions = context.Message.ActivityExceptions.Select(x => x.ExceptionInfo);
//                Fault<CreateOrder_RoutingSlipRequest> response =
//                    new FaultEvent<CreateOrder_RoutingSlipRequest>(request, requestId, context.Host, exceptions, TypeMetadataCache<CreateOrder_RoutingSlipRequest>.MessageTypeNames);
//                await responseEndpoint.Send(response);
//            }
//        }


//    }


//}
