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
    public class PaymentCreateSvcConsumer : IConsumer<PaymentCreateSvcCommand>
    {
        private readonly ILogger<PaymentCreateSvcConsumer> _logger;
        private readonly IMediator _mediator;

        public PaymentCreateSvcConsumer(
            ILogger<PaymentCreateSvcConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentCreateSvcCommand> context)
        {
            try
            {
                _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

                /* Создание платежа */
                var result = await _mediator.Send(new PaymentInitializeMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Price = context.Message.Price,
                    PaymentPlatform = context.Message.PaymentPlatform
                });

                await context.Publish(new PaymentCreateSuccessSvcEvent(context.Message.OrderID, result.PaymentID));
            }
            catch (Exception e)
            {
                await context.Publish(new PaymentCreateFaultSvcEvent(context.Message.OrderID, e));
            }

        }
    }
}
