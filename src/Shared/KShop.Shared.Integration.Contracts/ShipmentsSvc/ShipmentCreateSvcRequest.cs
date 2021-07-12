using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    public class ShipmentCreateSvcRequest
    {
        public ShipmentCreateSvcRequest(Guid orderID, List<ProductQuantity> orderContent, uint userID)
        {
            OrderID = orderID;
            OrderContent = orderContent;
            UserID = userID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public List<ProductQuantity> OrderContent { get; private set; }
    }

    public class ShipmentCreateSuccessSvcEvent
    {
        public ShipmentCreateSuccessSvcEvent(Guid orderID, Guid shipmentID)
        {
            OrderID = orderID;
            ShipmentID = shipmentID;
        }

        public Guid OrderID { get; private set; }
        public Guid ShipmentID { get; private set; }
    }

    public class ShipmentCreateFaultSvcEvent : BaseResponse
    {
        public ShipmentCreateFaultSvcEvent(Guid orderID) : base()
        {
            OrderID = orderID;
        }

        public ShipmentCreateFaultSvcEvent(Guid orderID, string errMsg) : base(errMsg)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }
}
