
using KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions;
using KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models;
using KShop.Shipments.Persistence;
using KShop.Shipments.Persistence.Entities;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.BackgroundServices
{
    /// <summary>
    /// Процедура отмены доставки во внешнем сервисе
    /// </summary>
    public class ShipmentCancellingBackgroundService : BackgroundService
    {
        private readonly ILogger<ShipmentCancellingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ShipmentCancellingBackgroundService(
            ILogger<ShipmentCancellingBackgroundService> logger,
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
                var db_context = scope.ServiceProvider.GetRequiredService<ShipmentContext>();
                var pub_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var shipment_provider = scope.ServiceProvider.GetRequiredService<IExternalShipmentServiceProvider>();

                var cancelling_shipments = await db_context.Shipments.Where(e => e.Status == EShipmentStatus.Cancelling).ToListAsync(stoppingToken);

                foreach (var shipment in cancelling_shipments)
                {
                    await shipment_provider.CancelShipmentAsync(
                        new ExternalShipmentCancelRequest
                        {
                            ExternalShipmnentID = shipment.ExternalID
                        });

                    shipment.Status = EShipmentStatus.Cancelled;
                    shipment.CompleteDate = DateTime.UtcNow;
                }

                await db_context.SaveChangesAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
