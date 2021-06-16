
using KShop.Shared.Domain.Contracts;

using KShop.Payments.Persistence;

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KShop.Shared.Integration.Contracts;

namespace KShop.Payments.Domain
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
                    Money = context.Message.Money,
                    PaymentPlatform = context.Message.PaymentPlatform
                });
            }
            catch (Exception e)
            {
                await context.Publish(new PaymentCreateFaultSvcEvent(context.Message.OrderID, e.Message));
            }

        }
    }
}
