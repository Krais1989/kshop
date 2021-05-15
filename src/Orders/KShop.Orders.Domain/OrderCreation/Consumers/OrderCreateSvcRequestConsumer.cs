using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Handlers;
using KShop.Orders.Persistence;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Consumers
{

    public class OrderCreateSvcRequestConsumer : IConsumer<OrderCreateSvcRequest>
    {
        private readonly ILogger<OrderCreateSvcRequestConsumer> _logger;
        private readonly IMediator _mediator;

        public OrderCreateSvcRequestConsumer(ILogger<OrderCreateSvcRequestConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderCreateSvcRequest> context)
        {
            try
            {
                var createOrder = new OrderCreateMediatorRequest()
                {
                    CustomerID = context.Message.CustomerID,
                    Positions = context.Message.Positions
                };

                var res = await _mediator.Send(createOrder);

                await context.RespondAsync(new OrderCreateSvcResponse()
                {
                    CorrelationID = context.CorrelationId.Value,
                    IsSuccess = true,
                    OrderID = res.OrderID
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync(new OrderCreateSvcResponse()
                {
                    CorrelationID = context.CorrelationId.Value,
                    IsSuccess = false,
                    OrderID = null,
                    Message = e.Message
                });
            }
        }
    }
}
