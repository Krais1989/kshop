using KShop.Communications.Contracts.Shipments;
using KShop.Shipments.Domain.Mediators;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.Consumers
{
    public class ShipmentCancelSvcConsumer : IConsumer<ShipmentCancelSvcRequest>
    {
        private readonly ILogger<ShipmentCancelSvcConsumer> _logger;
        private readonly IMediator _mediator;

        public ShipmentCancelSvcConsumer(
            ILogger<ShipmentCancelSvcConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ShipmentCancelSvcRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                /* Создание платежа */
                var result = await _mediator.Send(new ShipmentCancelMediatorRequest()
                {
                    ShipmentID = context.Message.ShipmentID
                });

                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new ShipmentCancelMediatorResponse()
                    {
                        
                    });
                }
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new ShipmentCancelMediatorResponse()
                    {
                        ErrorMessage = e.Message
                    });
                }

            }
        }
    }
}
