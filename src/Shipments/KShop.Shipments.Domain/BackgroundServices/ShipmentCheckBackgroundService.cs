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
    /// Процедура проверки состояния доставки во внешнем сервисе
    /// </summary>
    public class ShipmentCheckBackgroundService : BackgroundService
    {
        private readonly ILogger<ShipmentCheckBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExternalShipmentServiceProvider _externalShipment;

        public ShipmentCheckBackgroundService(
            ILogger<ShipmentCheckBackgroundService> logger,
            IServiceScopeFactory scopeFactory, IExternalShipmentServiceProvider externalShipment)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _externalShipment = externalShipment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            while (!stoppingToken.IsCancellationRequested)
            {
                var db_context = scope.ServiceProvider.GetRequiredService<ShipmentContext>();
                var pub_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var pending_shipments = await db_context.Shipments.Where(e => e.Status == EShipmentStatus.Pending).ToListAsync();

                foreach (var shipment in pending_shipments)
                {
                    var response = await _externalShipment.GetShipmentStatusAsync(
                        new ExternalShipmentGetStatusRequest()
                        {
                            ExternalShipmnentID = shipment.ExternalID
                        });

                    switch (response.Status)
                    {
                        case EExternalShipmentStatus.Processing:
                            shipment.Status = EShipmentStatus.Pending;
                            break;
                        case EExternalShipmentStatus.Shipped:
                            shipment.Status = EShipmentStatus.Shipped;
                            break;
                        case EExternalShipmentStatus.Cancelled:
                            shipment.Status = EShipmentStatus.Cancelled;
                            break;
                        case EExternalShipmentStatus.Faulted:
                            shipment.Status = EShipmentStatus.Faulted;
                            break;
                        default:
                            break;
                    }

                    if (response.Status != EExternalShipmentStatus.Processing)
                    {
                        shipment.CompleteDate = DateTime.UtcNow;
                    }
                }

                await db_context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
