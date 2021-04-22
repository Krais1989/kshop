using KShop.Communications.Contracts.Payments;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.BackgroundServices
{

    /// <summary>
    /// Инициализация
    /// </summary>
    public class PaymentInitBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCheckBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentInitBackgroundService(
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

                    var initPayments = await dbContext.Payments
                        .Where(e => e.Status == EPaymentStatus.Initializing)
                        .ToListAsync();

                    foreach (var payment in initPayments)
                    {
                        /* NOTE: логику синхронизации в медиатор? */
                        /* TODO: Запрос внешней системе на инициализацию платежа, запись внешнего ID платежа */
                        switch (payment.PaymentPlatformType)
                        {
                            case EPaymentPlatformType.None:
                                break;
                            case EPaymentPlatformType.Mock:
                                payment.ExternalPaymentID = Guid.NewGuid().ToString();
                                break;
                            case EPaymentPlatformType.Yookassa:
                                break;
                        }

                        payment.StatusDate = DateTime.UtcNow;
                        payment.Status = EPaymentStatus.Pending;
                        
                        await dbContext.AddAsync(new PaymentLog()
                        {
                            PaymentID = payment.ID,
                            ModifyDate = DateTime.UtcNow,
                            Status = EPaymentStatus.Pending,
                        });

                        await dbContext.SaveChangesAsync();
                    }


                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
