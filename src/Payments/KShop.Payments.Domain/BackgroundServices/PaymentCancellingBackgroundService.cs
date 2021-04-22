using KShop.Communications.Contracts.Payments;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.BackgroundServices
{
    /// <summary>
    /// Обработка отмены платежа
    /// </summary>
    public class PaymentCancellingBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCancellingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentCancellingBackgroundService(
            ILogger<PaymentCancellingBackgroundService> logger,
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
                        .Where(e => e.Status == EPaymentStatus.Cancelling)
                        .ToListAsync();

                    _logger.LogWarning($"Cancel payments number: {initPayments.Count}");
                    foreach (var payment in initPayments)
                    {
                        _logger.LogWarning($"\t{payment.ID} - {payment.PaymentPlatformType}");

                        /* TODO: Запрос внешней системе на отмену платежа */
                        switch (payment.PaymentPlatformType)
                        {
                            case EPaymentPlatformType.None:
                                break;
                            case EPaymentPlatformType.Mock:
                                break;
                            case EPaymentPlatformType.Yookassa:
                                break;
                        }

                        payment.StatusDate = DateTime.UtcNow;
                        payment.Status = EPaymentStatus.Canceled;

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
