
using KShop.Communications.Contracts.Shipments;
using KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions;
using KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models;
using KShop.Shipments.Persistence;
using KShop.Shipments.Persistence.Entities;
using MassTransit;
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

namespace KShop.Shipments.Domain.BackgroundServices
{

    /// <summary>
    /// Фоновый сервис для подтверждения оплаты
    /// </summary>
    public class ShipmentInitBackgroundService : BackgroundService
    {
        private readonly ILogger<ShipmentInitBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ShipmentInitBackgroundService(
            ILogger<ShipmentInitBackgroundService> logger,
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

                var init_shipments = await db_context.Shipments.Where(e => e.Status == EShipmentStatus.Initializing).ToListAsync();

                foreach (var shipment in init_shipments)
                {
                    try
                    {
                        var response = await shipment_provider.CreateShipmentAsync(
                            new ExternalShipmentCreateRequest()
                            {
                                OrderID = shipment.OrderID,
                                SourceAddress = "",
                                DestinationAddress = ""
                            });

                        shipment.PendingDate = DateTime.UtcNow;
                        shipment.Status = EShipmentStatus.Pending;
                        shipment.ExternalID = response.ExternalShipmnentID;

                        await db_context.SaveChangesAsync();

                        await pub_endpoint.Publish(new ShipmentCreateSuccessSvcEvent(shipment.OrderID, shipment.ID));
                    }
                    catch (Exception e)
                    {
                        await pub_endpoint.Publish(new ShipmentCreateFaultSvcEvent(shipment.OrderID, e));
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
