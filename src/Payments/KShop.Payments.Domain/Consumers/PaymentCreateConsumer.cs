using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
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
    public class PaymentCreateConsumer : IConsumer<PaymentCreateBusRequest>
    {
        private readonly ILogger<PaymentCreateConsumer> _logger;
        private readonly IMediator _mediator;

        public PaymentCreateConsumer(
            ILogger<PaymentCreateConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentCreateBusRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                /* Создание платежа */
                var result = await _mediator.Send(new PaymentCreateMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Price = context.Message.Price,
                    PaymentPlatform = context.Message.PaymentPlatform
                });

                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new PaymentCreateBusResponse()
                    {
                        PaymentID = result.PaymentID
                    });
                }
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new PaymentCreateBusResponse()
                    {
                        ErrorMessage = e.Message
                    });
                }

            }
        }
    }
}
