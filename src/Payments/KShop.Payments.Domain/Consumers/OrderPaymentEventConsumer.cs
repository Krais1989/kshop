using KShop.Communications.Contracts.Orders;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Consumers
{
    public class OrderPaymentEventConsumer : IConsumer<IOrderPayEvent>
    {
        private readonly ILogger<OrderPaymentEventConsumer> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly PaymentsContext _dbContext;

        public OrderPaymentEventConsumer(ILogger<OrderPaymentEventConsumer> logger, IPublishEndpoint pubEndpoint, PaymentsContext dbContext)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IOrderPayEvent> context)
        {
            _logger.LogInformation($"{context.Message.GetType().Name}: {JsonSerializer.Serialize(context.Message)}");
            try
            {
                /* Создание платежа */
                var payment = new Payment()
                {
                    ID = context.Message.OrderID,
                    StatusDate = DateTime.UtcNow,
                    Status = Payment.EStatus.Pending,
                    Logs = new List<PaymentLog> {
                        new PaymentLog()
                        {
                            ModifyDate = DateTime.UtcNow,
                            Status = Payment.EStatus.Pending
                        }
                    }
                };

                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await _pubEndpoint.Publish(new OrderPayFailureEvent() { OrderID = context.Message.OrderID });
            }
        }
    }
}
