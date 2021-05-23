namespace KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models
{
    public class ExternalShipmentGetStatusResponse : ExternalShipmentBaseResponse
    {
        public EExternalShipmentStatus Status { get; set; }
    }
}
