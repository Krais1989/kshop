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
    public class PaymentCheckBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCheckBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentCheckBackgroundService(
            ILogger<PaymentCheckBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
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
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    var pendingPayments = await dbContext.Payments.AsNoTracking()
                        .Where(e => e.Status == EPaymentStatus.Pending)
                        .ToListAsync();

                    foreach (var payment in pendingPayments)
                    {
                        var payReq = new PaymentExternalPayMediatorRequest() { 
                            PlatformType = payment.PaymentPlatformType, 
                            ExternalPaymentID = payment.ExternalPaymentID 
                        };
                        await mediator.Send(payReq);
                        //await publishEndpoint.Publish(new ExternalPaymentStatusChanged
                        //{ 
                        //    OrderID = payment.OrderID,
                        //    NewStatus = EInvoiceStatus.Paid
                        //});
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }


    }
}
