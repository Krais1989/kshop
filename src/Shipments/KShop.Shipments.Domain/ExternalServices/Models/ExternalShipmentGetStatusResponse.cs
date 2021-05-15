namespace KShop.Shipments.Domain.ExternalServices
{
    public class ExternalShipmentGetStatusResponse
    {
        public bool IsSuccess { get; set; }
        public EExternalShipmentStatus Status { get; set; }
    }
}
