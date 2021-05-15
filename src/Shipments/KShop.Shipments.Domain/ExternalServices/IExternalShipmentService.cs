using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.ExternalServices
{
    public enum EExternalShipmentStatus
    {
        Processing,
        Shipped,
        Cancelled,
        Faulted
    }

    public interface IExternalShipmentService
    {
        public Task<ExternalShipmentCreateResponse> CreateShipmentAsync(ExternalShipmentCreateRequest request, CancellationToken cancellationToken = default);
        public Task<ExternalShipmentCancelResponse> CancelShipmentAsync(ExternalShipmentCancelRequest request, CancellationToken cancellationToken = default);
        public Task<ExternalShipmentGetStatusResponse> GetShipmentStatusAsync(ExternalShipmentGetStatusRequest request, CancellationToken cancellationToken = default);
    }
}
