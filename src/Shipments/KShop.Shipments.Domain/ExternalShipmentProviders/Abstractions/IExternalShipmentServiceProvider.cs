using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain
{

    public interface IExternalShipmentServiceProvider
    {
        public Task<ExternalShipmentCreateResponse> CreateShipmentAsync(ExternalShipmentCreateRequest request, CancellationToken cancellationToken = default);
        public Task<ExternalShipmentCancelResponse> CancelShipmentAsync(ExternalShipmentCancelRequest request, CancellationToken cancellationToken = default);
        public Task<ExternalShipmentGetStatusResponse> GetShipmentStatusAsync(ExternalShipmentGetStatusRequest request, CancellationToken cancellationToken = default);
    }
}
