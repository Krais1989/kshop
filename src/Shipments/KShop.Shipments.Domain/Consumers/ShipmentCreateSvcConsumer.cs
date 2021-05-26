using KShop.Communications.Contracts.Shipments;
using KShop.Shipments.Domain.Mediators;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.Consumers
{
    public class ShipmentCreateSvcConsumer : IConsumer<ShipmentCreateSvcCommand>
    {
        private readonly ILogger<ShipmentCreateSvcConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public ShipmentCreateSvcConsumer(
            ILogger<ShipmentCreateSvcConsumer> logger,
            IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ShipmentCreateSvcCommand> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                var result = await _mediator.Send(new ShipmentInitializeMediatorRequest()
                {
                    OrderID = context.Message.OrderID
                });
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish(new ShipmentCreateFaultSvcEvent(context.Message.OrderID, e.Message));
            }
        }
    }
}
