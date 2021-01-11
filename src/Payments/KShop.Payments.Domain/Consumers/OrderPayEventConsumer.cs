using KShop.Communications.Contracts.Orders;
using KShop.Payments.Domain.Mediators;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Consumers
{
    /// <summary>
    /// Обработки событий 
    /// </summary>
    public class OrderPayEventConsumer : IConsumer<IOrderPayEvent>
    {
        private readonly ILogger<OrderPayEventConsumer> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly IMediator _mediator;

        public OrderPayEventConsumer(ILogger<OrderPayEventConsumer> logger, IPublishEndpoint pubEndpoint, IMediator mediator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IOrderPayEvent> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");
            try
            {
                /* Создание платежа */
                var result = await _mediator.Send(new PaymentCreateMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Price = context.Message.Price,
                });
            }
            catch (Exception e)
            {
                await _pubEndpoint.Publish(new OrderPayFailureEvent() { OrderID = context.Message.OrderID });
            }
        }
    }
}
