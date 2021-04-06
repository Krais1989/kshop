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
    public class OrderCreate_RequestConsumer : IConsumer<OrderCreate_BusRequest>
    {
        private readonly ILogger<OrderCreate_RequestConsumer> _logger;
        private readonly IMediator _mediator;

        public OrderCreate_RequestConsumer(ILogger<OrderCreate_RequestConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderCreate_BusRequest> context)
        {
            try
            {
                var createOrder = new OrderCreateMediatorRequest()
                {
                    CustomerID = context.Message.CustomerID,
                    Positions = context.Message.Positions
                };

                var res = await _mediator.Send(createOrder);

                await context.RespondAsync(new OrderCreate_BusResponse()
                {
                    CorrelationID = context.CorrelationId.Value,
                    IsSuccess = true,
                    OrderID = res.OrderID
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync(new OrderCreate_BusResponse()
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
