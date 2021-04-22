using KShop.Communications.Contracts.Invoices;
using KShop.Payments.Domain.Mediators;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Consumers
{
    public class PaymentCancelConsumer : IConsumer<PaymentCancelBusRequest>
    {
        private readonly ILogger<PaymentCancelConsumer> _logger;
        private readonly IMediator _mediator;

        public PaymentCancelConsumer(
            ILogger<PaymentCancelConsumer> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PaymentCancelBusRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");

            try
            {
                var result = await _mediator.Send(new PaymentCancelMediatorRequest()
                {
                    PaymentID = context.Message.CorrelationID
                });
                //await context.RespondAsync(new InvoiceCreate_BusResponse());
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                {
                    await context.RespondAsync(new PaymentProceedFailure()
                    {
                        CorrelationID = context.CorrelationId.Value,
                        Reason = PaymentProceedFailure.EReason.InternalError,
                        Message = e.Message
                    });
                }
            }
        }
    }
}
