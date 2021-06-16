using KShop.Shared.Domain.Contracts;
using KShop.Payments.Persistence;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
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
            using var scope = _scopeFactory.CreateScope();

            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("=== ExecuteAsync ===");
                var db_context = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
                var pub_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var payment_provider = scope.ServiceProvider.GetRequiredService<ICommonPaymentProvider>();

                var cancelling_payments = await db_context.Payments
                    .Where(e => e.Status == EPaymentStatus.Cancelling)
                    .ToListAsync();

                foreach (var payment in cancelling_payments)
                {
                    try
                    {
                        _logger.LogWarning($"Cancel: {payment.ID} \tOrder: {payment.OrderID}");

                        var result = await payment_provider.CancelAsync(
                            new CommonPaymentProviderCancelRequest
                            {
                                ExternalPaymentID = payment.ExternalID,
                                Provider = payment.PaymentProvider
                            });

                        payment.SetStatus(EPaymentStatus.Canceled);

                        await db_context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Exception when cancelling Payment: {payment.ID}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
