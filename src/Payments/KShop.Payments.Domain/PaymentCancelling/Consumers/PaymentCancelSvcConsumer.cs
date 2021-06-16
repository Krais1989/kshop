using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{
    public class PaymentCancelSvcConsumer : IConsumer<PaymentCancelSvcRequest>
    {
        private readonly ILogger<PaymentCancelSvcConsumer> _logger;
        private readonly IMediator _mediator;

        public PaymentCancelSvcConsumer(
            ILogger<PaymentCancelSvcConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentCancelSvcRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                var result = await _mediator.Send(new PaymentSetCanceledMediatorRequest()
                {
                    PaymentID = context.Message.PaymentID
                });

                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new PaymentCancelSvcResponse()
                    {
                    });
                }
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new PaymentCancelSvcResponse()
                    {
                        ErrorMessage = e.Message
                    });
                }
            }
        }
    }
}
