using KShop.Shared.Domain.Contracts;
using KShop.Shipments.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain
{
    public class MockExternallShipmentProvider : IExternalShipmentServiceProvider
    {
        public async Task<ExternalShipmentCancelResponse> CancelShipmentAsync(ExternalShipmentCancelRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentCancelResponse
            {
            };
            return result;
        }

        public async Task<ExternalShipmentCreateResponse> CreateShipmentAsync(ExternalShipmentCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentCreateResponse
            {
                ExternalShipmnentID = Guid.NewGuid().ToString()
            };
            return result;
        }

        public async Task<ExternalShipmentGetStatusResponse> GetShipmentStatusAsync(ExternalShipmentGetStatusRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentGetStatusResponse
            {
                ShipmentStatus = EShipmentStatus.Shipped
            };
            return result;
        }
    }
}
