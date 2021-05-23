using System;

namespace KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models
{
    public class ExternalShipmentCreateRequest
    {
        public Guid OrderID { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
    }
}
