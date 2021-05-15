using KShop.Communications.Contracts.Shipments;
using KShop.Shipments.Domain.ShipmentProcessing.Mediators;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.ShipmentProcessing.Consumers
{
    public class ShipmentCreateSvcConsumer : IConsumer<ShipmentCreateSvcRequest>
    {
        private readonly ILogger<ShipmentCreateSvcConsumer> _logger;
        private readonly IMediator _mediator;

        public ShipmentCreateSvcConsumer(
            ILogger<ShipmentCreateSvcConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ShipmentCreateSvcRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                /* Создание платежа */
                var result = await _mediator.Send(new ShipmentCreateMediatorRequest()
                {
                    OrderID = context.Message.OrderID
                });

                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new ShipmentCreateMediatorResponse()
                    {
                        ShipmentID = result.ShipmentID
                    });
                }
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new ShipmentCreateMediatorResponse()
                    {
                        ErrorMessage = e.Message
                    });
                }

            }
        }
    }
}
