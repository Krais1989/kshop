using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    public class ShipmentCreateSvcCommand
    {
        public Guid OrderID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
    }

    public class ShipmentCreateSuccessSvcEvent
    {
        public ShipmentCreateSuccessSvcEvent(Guid orderID, Guid shipmentID)
        {
            OrderID = orderID;
            ShipmentID = shipmentID;
        }

        public Guid OrderID { get; set; }
        public Guid ShipmentID { get; set; }
    }

    public class ShipmentCreateFaultSvcEvent
    {
        public ShipmentCreateFaultSvcEvent(Guid orderID, string errorMessage = null)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }

        public Guid OrderID { get; set; }
        public string ErrorMessage { get; set; }
    }
}
