using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Domain.ExternalShipmentProviders.Abstractions.Models
{
    public abstract class ExternalShipmentBaseResponse
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
