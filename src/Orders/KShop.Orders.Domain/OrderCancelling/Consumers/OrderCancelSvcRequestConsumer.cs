//
//using KShop.Orders.Domain.OrderCancelling.Mediators;
//using MassTransit;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
//namespace KShop.Orders.Domain.OrderCancelling.Consumers
//{
//    public class OrderCancelSvcRequestConsumer : IConsumer<OrderCancelSvcRequest>
//    {
//        private readonly ILogger<OrderCancelSvcRequestConsumer> _logger;
//        private readonly IMediator _mediator;

//        public OrderCancelSvcRequestConsumer(ILogger<OrderCancelSvcRequestConsumer> logger, IMediator mediator)
//        {
//            _logger = logger;
//            _mediator = mediator;
//        }

//        public async Task Consume(ConsumeContext<OrderCancelSvcRequest> context)
//        {
//            try
//            {
//                var createOrder = new OrderCancelMediatorRequest()
//                {
//                    OrderID = context.Message.OrderID
//                };

//                var res = await _mediator.Send(createOrder);

//                //TODO: убрать синхронный подход
//                if (context.RequestId.HasValue && context.ResponseAddress != null)
//                    await context.RespondAsync(new OrderCancelSvcResponse(context.Message.OrderID));
//            }
//            catch (Exception e)
//            {
//                if (context.RequestId.HasValue && context.ResponseAddress != null)
//                    await context.RespondAsync(new OrderCancelSvcResponse(context.Message.OrderID)
//                    {
//                        ErrorMessage = e.Message
//                    });
//            }
//        }
//    }
//}
}