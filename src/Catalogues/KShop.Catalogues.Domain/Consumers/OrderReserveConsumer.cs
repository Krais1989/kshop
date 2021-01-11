using KShop.Catalogues.Domain.Exceptions;
using KShop.Catalogues.Domain.Mediators;
using KShop.Catalogues.Persistence;
using KShop.Catalogues.Persistence.Entities;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Catalogues.Domain.Consumers
{
    public interface ICompensate<out T> { }

    public class OrderReserveConsumer : 
        IConsumer<IOrderReserveEvent>, 
        IConsumer<Fault<IOrderReserveEvent>>,
        IConsumer<ICompensate<IOrderReserveEvent>>
    {
        private readonly ILogger<OrderReserveConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;

        public OrderReserveConsumer(
            ILogger<OrderReserveConsumer> logger,
            IPublishEndpoint publishEndpoint, IMediator mediator)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IOrderReserveEvent> context)
        {
            _logger.LogInformation($"EXECUTE: {context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");
            try
            {
                await _mediator.Send(new OrderReserveMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Positions = context.Message.Positions
                });

                await _publishEndpoint.Publish<IOrderReserveSuccessEvent>(new { OrderID = context.Message.OrderID });
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish<IOrderReserveFailureEvent>(new { OrderID = context.Message.OrderID });
                throw;
            }
        }

        public Task Consume(ConsumeContext<Fault<IOrderReserveEvent>> context)
        {
            _logger.LogInformation($"FAULT:{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");
            throw new NotImplementedException();
        }

        public Task Consume(ConsumeContext<ICompensate<IOrderReserveEvent>> context)
        {
            throw new NotImplementedException();
        }
    }
}
