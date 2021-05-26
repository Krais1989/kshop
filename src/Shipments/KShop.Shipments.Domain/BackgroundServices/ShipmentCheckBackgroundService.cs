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

        public ShipmentCheckBackgroundService(
            ILogger<ShipmentCheckBackgroundService> logger,
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
                var db_context = scope.ServiceProvider.GetRequiredService<ShipmentContext>();
                var pub_endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var shipment_provider = scope.ServiceProvider.GetRequiredService<IExternalShipmentServiceProvider>();

                var pending_shipments = await db_context.Shipments
                    .AsNoTracking()
                    .Where(e => e.Status == EShipmentStatus.Pending)
                    .OrderBy(e => e.LastCheckingDate)
                    .Take(500)
                    .ToListAsync();

                foreach (var shipment in pending_shipments)
                {
                    try
                    {
                        _logger.LogWarning($"Check Shipment: {shipment.ID} \tOrder: {shipment.OrderID}");

                        if (string.IsNullOrEmpty(shipment.ExternalID))
                        {
                            shipment.SetStatus(EShipmentStatus.Error);
                            db_context.Update(shipment);
                            await db_context.SaveChangesAsync();
                            continue;
                        }

                        var check_response = await shipment_provider.GetShipmentStatusAsync(
                            new ExternalShipmentGetStatusRequest()
                            {
                                ExternalShipmnentID = shipment.ExternalID
                            });

                        switch (check_response.ShipmentStatus)
                        {
                            case EShipmentStatus.Pending:
                            case EShipmentStatus.Shipped:
                            case EShipmentStatus.Cancelling:
                            case EShipmentStatus.Cancelled:
                            case EShipmentStatus.Error:
                                shipment.SetStatus(check_response.ShipmentStatus);
                                db_context.Update(shipment);
                                break;
                            default:
                                _logger.LogWarning($"{shipment.ID} Shipment check skiped because of status: {check_response.ShipmentStatus}");
                                break;
                        }

                        if (check_response.ShipmentStatus == EShipmentStatus.Shipped)
                            await pub_endpoint.Publish(new ShipmentCreateSuccessSvcEvent(shipment.OrderID, shipment.ID));
                        if (check_response.ShipmentStatus == EShipmentStatus.Cancelled || check_response.ShipmentStatus == EShipmentStatus.Error)
                            await pub_endpoint.Publish(new ShipmentCreateFaultSvcEvent(shipment.OrderID, $"Shipemt external faulted with status {check_response.ShipmentStatus}"));

                        await db_context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Exception when checking Shipment ID: {shipment.ID}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
