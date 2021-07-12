using KShop.Orders.Persistence;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderCreateSvcRequestConsumer : IConsumer<OrderCreateSvcRequest>
    {
        private readonly ILogger<OrderCreateSvcRequestConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreateSvcRequestConsumer(ILogger<OrderCreateSvcRequestConsumer> logger, IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreateSvcRequest> context)
        {
            try
            {
                var createOrder = new OrderCreateMediatorRequest
                (
                    orderID: context.Message.OrderID,
                    customerID: context.Message.UserID,
                    orderContent: context.Message.OrderContent,
                    orderPrice: context.Message.OrderPrice
                );

                var res = await _mediator.Send(createOrder);

                await _publishEndpoint.Publish(new OrderCreateSuccessSvcEvent(orderID: res.OrderID));

                //if (context.RequestId.HasValue && context.ResponseAddress != null)
                //    await context.RespondAsync(new OrderCreateSuccessSvcEvent() { OrderID = res.OrderID });
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish(new OrderCreateFaultSvcEvent(context.Message.OrderID));
                //if (context.RequestId.HasValue && context.ResponseAddress != null)
                //    await context.RespondAsync(new OrderCreateSuccessSvcEvent() { ErrorMessage = e.Message });
            }
        }
    }
}
