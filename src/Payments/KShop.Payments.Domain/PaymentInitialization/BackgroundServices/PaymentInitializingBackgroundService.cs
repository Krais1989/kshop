using KShop.Shared.Domain.Contracts;
using KShop.Payments.Persistence;

using MassTransit;
using MassTransit.Events;
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

namespace KShop.Payments.Domain
{

    /// <summary>
    /// Инициализация
    /// </summary>
    public class PaymentInitializingBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCheckingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentInitializingBackgroundService(
            ILogger<PaymentCheckingBackgroundService> logger,
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

                var initilizing_payments = await db_context.Payments
                    .AsNoTracking()
                    .Where(e => e.Status == EPaymentStatus.Initializing)
                    .ToListAsync();

                foreach (var payment in initilizing_payments)
                {
                    try
                    {
                        _logger.LogWarning($"Initialize: {payment.ID} \tOrder: {payment.OrderID}");
                        var provider_result = await payment_provider.CreateAsync(
                            new CommonPaymentProviderCreateRequest()
                            {
                                OrderID = payment.OrderID,
                                Provider = payment.PaymentProvider,
                                Money = payment.Money
                            });

                        payment.ExternalID = provider_result.ExternalPaymentID;
                        payment.SetStatus(EPaymentStatus.Pending);
                        db_context.Update(payment);
                        await db_context.SaveChangesAsync();

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Exception when initializing Payment: {payment.ID}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
