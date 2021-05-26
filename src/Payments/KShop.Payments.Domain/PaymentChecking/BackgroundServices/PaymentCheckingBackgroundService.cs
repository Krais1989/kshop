using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
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

        public PaymentCheckingBackgroundService(
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
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var payment_provider = scope.ServiceProvider.GetRequiredService<ICommonPaymentProvider>();

                // TODO: хранить дату последней проверки платежа, чтобы запрашивать по убыванию этой даты
                /* Иначе при порционных запросах будут выбираться  */

                var pending_payments = await db_context.Payments
                    .AsNoTracking()
                    .Where(e => e.Status == EPaymentStatus.Pending)
                    .OrderBy(e => e.LastCheckingDate)
                    .Take(500)
                    .ToListAsync();

                foreach (var payment in pending_payments)
                {
                    try
                    {
                        _logger.LogWarning($"Check Payment: {payment.ID} \tOrder: {payment.OrderID}");

                        if (string.IsNullOrEmpty(payment.ExternalID))
                        {
                            payment.SetStatus(EPaymentStatus.Error);
                            db_context.Update(payment);
                            await db_context.SaveChangesAsync();
                            continue;
                        }

                        var check_response = await payment_provider.GetStatusAsync(
                            new CommonPaymentProviderGetStatusRequest
                            {
                                Provider = payment.PaymentProvider
                            });

                        switch (check_response.PaymentStatus)
                        {
                            case EPaymentStatus.Pending:
                            case EPaymentStatus.Paid:
                            case EPaymentStatus.Cancelling:
                            case EPaymentStatus.Canceled:
                            case EPaymentStatus.Rejected:
                            case EPaymentStatus.Error:
                                payment.SetStatus(check_response.PaymentStatus);
                                db_context.Update(payment);
                                break;
                            default:
                                _logger.LogWarning($"{payment.ID} Payment check skiped because of status: {check_response.PaymentStatus}");
                                break;
                        }

                        if (check_response.PaymentStatus == EPaymentStatus.Paid)
                            await pub_endpoint.Publish(new PaymentCreateSuccessSvcEvent(payment.OrderID, payment.ID));
                        if (check_response.PaymentStatus == EPaymentStatus.Error || check_response.PaymentStatus == EPaymentStatus.Canceled)
                            await pub_endpoint.Publish(new PaymentCreateFaultSvcEvent(payment.OrderID, $"Payment external faulted with status {check_response.PaymentStatus}"));

                        await db_context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Exception when checking Payment: {payment.ID}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

        }


    }
}
