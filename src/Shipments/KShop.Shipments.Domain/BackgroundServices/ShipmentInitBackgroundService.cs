
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.BackgroundServices
{

    /// <summary>
    /// Фоновый сервис для подтверждения оплаты
    /// </summary>
    public class ShipmentInitBackgroundService : BackgroundService
    {
        private readonly ILogger<ShipmentInitBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IExternalShipmentServiceProvider _externalShipment;

        public ShipmentInitBackgroundService(
            ILogger<ShipmentInitBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            IExternalShipmentServiceProvider externalShipment)
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
                //_logger.LogInformation("=== ExecuteAsync ===");
                var db_context = scope.ServiceProvider.GetRequiredService<ShipmentContext>();
                var pub_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var init_shipments = await db_context.Shipments.Where(e => e.Status == EShipmentStatus.Initializing).ToListAsync();

                foreach (var shipment in init_shipments)
                {
                    var response = await _externalShipment.CreateShipmentAsync(
                        new ExternalShipmentCreateRequest()
                        {
                            OrderID = shipment.OrderID,
                            SourceAddress = "",
                            DestinationAddress = ""
                        });

                    shipment.PendingDate = DateTime.UtcNow;
                    shipment.Status = EShipmentStatus.Pending;
                    shipment.ExternalID = response.ExternalShipmnentID;
                }

                await db_context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
