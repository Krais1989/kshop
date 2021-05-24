﻿using KShop.Communications.Contracts.Payments;
using KShop.Payments.Domain.ExternalPaymentProviders.Common;
using KShop.Payments.Domain.ExternalPaymentProviders.Common.Models;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
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

namespace KShop.Payments.Domain.BackgroundServices
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
                    .Where(e => e.Status == EPaymentStatus.Initializing)
                    .ToListAsync();

                _logger.LogWarning($"Initializing payments count: {initilizing_payments.Count}");
                foreach (var payment in initilizing_payments)
                {
                    try
                    {
                        _logger.LogWarning($"Initialize: {payment.ID} \tOrder: {payment.OrderID}");
                        var provider_result = await payment_provider.CreateAsync(
                            new CommonPaymentProviderCreateRequest()
                            {
                                OrderID = payment.OrderID,
                                Provider = payment.PaymentProvider
                            });

                        payment.SetStatus(EPaymentStatus.Pending);
                        await db_context.SaveChangesAsync();

                        await pub_endpoint.Publish(new PaymentCreateSuccessSvcEvent(payment.OrderID, payment.ID));

                    }
                    catch (Exception e)
                    {
                        await pub_endpoint.Publish(new PaymentCreateFaultSvcEvent(payment.OrderID, e));
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
