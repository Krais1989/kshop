using KShop.Communications.Contracts.Invoices;
using KShop.Communications.Contracts.Orders;
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
    /// <summary>
    /// Обработки событий 
    /// </summary>
    public class InvoceCreateConsumer : IConsumer<InvoiceCreate_BusRequest>
    {
        private readonly ILogger<InvoceCreateConsumer> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly IMediator _mediator;

        public InvoceCreateConsumer(ILogger<InvoceCreateConsumer> logger, IPublishEndpoint pubEndpoint, IMediator mediator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<InvoiceCreate_BusRequest> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");
            try
            {
                /* Создание платежа */
                var result = await _mediator.Send(new PaymentCreateMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Price = context.Message.Price,
                });
                await context.RespondAsync(new InvoiceCreate_BusResponse());
            }
            catch (Exception e)
            {
                await context.RespondAsync(new InvoiceCreate_BusResponse(false, e.Message));
            }
        }
    }
}
