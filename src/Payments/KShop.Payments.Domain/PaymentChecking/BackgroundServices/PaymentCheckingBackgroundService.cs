using KShop.Communications.Contracts.Orders;
using KShop.Payments.Domain.ExternalPaymentProviders.Common;
using KShop.Payments.Domain.ExternalPaymentProviders.Common.Models;
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
    /// Фоновый сервис запрашивающий состояния платежа у внешней системы
    /// </summary>
    public class PaymentCheckingBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCheckingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICommonPaymentProvider _paymentProvider;

        public PaymentCheckingBackgroundService(
            ILogger<PaymentCheckingBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            ICommonPaymentProvider paymentProvider)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _paymentProvider = paymentProvider;
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
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var pending_payments = await db_context.Payments
                    .Where(e => e.Status == EPaymentStatus.Pending)
                    .Where(e => !string.IsNullOrEmpty(e.ExternalPaymentID))
                    .ToListAsync();

                _logger.LogWarning($"Checking payments count: {pending_payments.Count}");
                foreach (var payment in pending_payments)
                {
                    _logger.LogWarning($"Check: {payment.ID} \tOrder: {payment.OrderID}");
                    var provider_result = await _paymentProvider.GetStatusAsync(
                        new CommonPaymentProviderGetStatusRequest
                        {
                            Provider = payment.PaymentProvider
                        });

                    if (provider_result.PaymentStatus == EPaymentStatus.Paid 
                        || provider_result.PaymentStatus == EPaymentStatus.Canceled
                        || provider_result.PaymentStatus == EPaymentStatus.Error)
                    {
                        payment.SetStatus(provider_result.PaymentStatus);
                        db_context.Update(payment);
                    }
                }

                await db_context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

        }


    }
}
