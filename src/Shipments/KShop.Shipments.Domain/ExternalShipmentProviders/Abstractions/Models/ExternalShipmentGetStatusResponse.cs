using KShop.Shipments.Persistence.Entities;

namespace KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models
{
    public class ExternalShipmentGetStatusResponse : ExternalShipmentBaseResponse
    {
        public EShipmentStatus ShipmentStatus { get; set; }
    }
}
