using KShop.Communications.Contracts.Orders;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MassTransit;
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
    /// Фоновый сервис для проверки состояния платежа
    /// </summary>
    public class PaymentCheckBackgroundService : BackgroundService
    {
        private readonly ILogger<PaymentCheckBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentCheckBackgroundService(ILogger<PaymentCheckBackgroundService> logger, IServiceScopeFactory scopeFactory)
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

                    await TestAutoApprove(dbContext, bus, stoppingToken);

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        protected async Task TestAutoApprove(PaymentsContext dbContext, IBusControl publishEndpoint, CancellationToken stoppingToken)
        {
            //_logger.LogInformation($"\tTEST - Auto Approve Payments");

            var pendingPayments = await dbContext.Payments
                .Include(e => e.Logs)
                .Where(e => e.Status == Payment.EStatus.Pending)
                .ToListAsync();

            foreach (var exPayment in pendingPayments)
            {
                var newStatus = Payment.EStatus.Success;
                exPayment.Status = newStatus;
                exPayment.StatusDate = DateTime.UtcNow;
                exPayment.Logs.Add(new PaymentLog() { ModifyDate = DateTime.UtcNow, Status = newStatus });
                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation($"\t\t>>> {exPayment.ID} {exPayment.StatusDate} ");

                //await publishEndpoint.Publish(new OrderPaySuccessEvent() { OrderID = exPayment.ID });
                await publishEndpoint.Publish<IOrderPaySuccessEvent>(new { OrderID = exPayment.ID });
            }

            
        }

    }
}
