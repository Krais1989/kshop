using KShop.Communications.Contracts.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Shipments
{
    public class ShipmentCreateSvcCommand
    {
        public Guid OrderID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
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
        public ShipmentCreateFaultSvcEvent(Guid orderID, Exception exception = null)
        {
            OrderID = orderID;
            Exception = exception;
        }

        public ShipmentCreateFaultSvcEvent(Guid orderID, string errorMessage)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }

        public Guid OrderID { get; set; }
        public Exception Exception { get; set; }
        public string ErrorMessage { get; set; }
    }
}
