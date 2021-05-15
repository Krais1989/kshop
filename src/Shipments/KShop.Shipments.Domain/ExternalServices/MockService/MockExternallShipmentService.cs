using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.ExternalServices
{
    public class MockExternallShipmentService : IExternalShipmentService
    {
        public async Task<ExternalShipmentCancelResponse> CancelShipmentAsync(ExternalShipmentCancelRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentCancelResponse
            {
                IsSuccess = true
            };
            return result;
        }

        public async Task<ExternalShipmentCreateResponse> CreateShipmentAsync(ExternalShipmentCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentCreateResponse
            {
                IsSuccess = true,
                ExternalShipmnentID = Guid.NewGuid().ToString()
            };
            return result;
        }

        public async Task<ExternalShipmentGetStatusResponse> GetShipmentStatusAsync(ExternalShipmentGetStatusRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ExternalShipmentGetStatusResponse
            {
                IsSuccess = true,
                Status = EExternalShipmentStatus.Shipped
            };
            return result;
        }
    }
}
