using KShop.Communications.Contracts.Enums;
using KShop.Communications.Contracts.Invoices;
using KShop.Communications.Contracts.Orders;
using KShop.Payments.Domain.Mediators;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.BackgroundServices
{
    /// <summary>
    /// Фоновый сервис для подтверждения оплаты
    /// </summary>
    public class PaymentApproveBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentApproveBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMediator _mediator;

        public PaymentApproveBackgroundService(
            ILogger<PaymentApproveBackgroundService> logger, 
            IServiceScopeFactory scopeFactory,
            IMediator mediator)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //_logger.LogInformation("=== ExecuteAsync ===");
                    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();

                    await TestAutoApprove(dbContext, bus, stoppingToken);

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        protected async Task TestAutoApprove(PaymentsContext dbContext, IBusControl publishEndpoint, CancellationToken stoppingToken)
        {
            //_logger.LogInformation($"\tTEST - Auto Approve Payments");

            var pendingPayments = await dbContext.Payments
                .Where(e => e.Status == Payment.EStatus.Pending)
                .ToListAsync();

            /* ТЕСТ: подтвержает все Pending заказы*/
            /* НУЖНО: нужно опрашивать внешний сервис оплаты */
            foreach (var payment in pendingPayments)
            {
                await _mediator.Send(new PaymentApproveMediatorRequest() { PaymentID = payment.ID });
                await publishEndpoint.Publish(new InvoiceStatusChanged_BusEvent { 
                    OrderID = payment.OrderID,
                    InvoiceStatus = EInvoiceStatus.Paid
                });
            }            
        }

    }
}
