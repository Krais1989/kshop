
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
    public class ShipmentInitializingBackgroundService : BackgroundService
    {
        private readonly ILogger<ShipmentInitializingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ShipmentInitializingBackgroundService(
            ILogger<ShipmentInitializingBackgroundService> logger,
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

                var init_shipments = await db_context.Shipments
                    .AsNoTracking()
                    .Where(e => e.Status == EShipmentStatus.Initializing)
                    .ToListAsync();

                foreach (var shipment in init_shipments)
                {
                    try
                    {
                        _logger.LogWarning($"Initialize shipment: {shipment.ID} \tOrder: {shipment.OrderID}");
                        var response = await shipment_provider.CreateShipmentAsync(
                            new ExternalShipmentCreateRequest()
                            {
                                OrderID = shipment.OrderID,
                                SourceAddress = "",
                                DestinationAddress = ""
                            });

                        shipment.ExternalID = response.ExternalShipmnentID;
                        shipment.SetStatus(EShipmentStatus.Pending);

                        db_context.Update(shipment);
                        await db_context.SaveChangesAsync();
                    }
                    // TODO: отлавливать отдельный тип исключения, приводящий к Fault статусу платежа, вместо скипа и повтора
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Exception when initializing Shipment: {shipment.ID}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
