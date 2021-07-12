

using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderCancelSvcRequestConsumer : IConsumer<OrderCancelSvcRequest>
    {
        private readonly ILogger<OrderCancelSvcRequestConsumer> _logger;
        private readonly IMediator _mediator;

        public OrderCancelSvcRequestConsumer(ILogger<OrderCancelSvcRequestConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderCancelSvcRequest> context)
        {
            try
            {
                var createOrder = new OrderCancelMediatorRequest
                (
                    orderID: context.Message.OrderID,
                    userID: context.Message.UserID
                );

                var res = await _mediator.Send(createOrder);

                //TODO: убрать синхронный подход
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                    await context.RespondAsync(new OrderCancelSvcResponse());
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                    await context.RespondAsync(new OrderCancelSvcResponse(e.Message));
            }
        }
    }

}