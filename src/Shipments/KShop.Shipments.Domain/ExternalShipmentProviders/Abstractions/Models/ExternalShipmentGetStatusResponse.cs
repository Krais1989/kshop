using KShop.Shared.Domain.Contracts;
using KShop.Shipments.Persistence;

namespace KShop.Shipments.Domain
{
    public class ExternalShipmentGetStatusResponse : ExternalShipmentBaseResponse
    {
        public EShipmentStatus ShipmentStatus { get; set; }
    }
}
