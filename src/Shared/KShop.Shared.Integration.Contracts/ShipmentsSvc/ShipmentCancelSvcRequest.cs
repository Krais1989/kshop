using System;

namespace KShop.Shared.Integration.Contracts
{
    public class ShipmentCancelSvcRequest
    {
        public ShipmentCancelSvcRequest(Guid shipmentID, uint userID)
        {
            UserID = userID;
            ShipmentID = shipmentID;
        }

        public uint UserID { get; private set; }
        public Guid ShipmentID { get; private set; }
    }
}
