using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain
{
    public class ShipmentCreateSvcConsumer : IConsumer<ShipmentCreateSvcRequest>
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

        public async Task Consume(ConsumeContext<ShipmentCreateSvcRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                var result = await _mediator.Send(new ShipmentInitializeMediatorRequest
                (
                    orderID: context.Message.OrderID,
                    userID: context.Message.UserID
                ));
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish(new ShipmentCreateFaultSvcEvent(context.Message.OrderID, e.Message));
            }
        }
    }
}
