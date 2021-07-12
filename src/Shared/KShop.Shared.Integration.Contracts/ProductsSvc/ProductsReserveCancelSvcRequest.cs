using System;

namespace KShop.Shared.Integration.Contracts
{
    public class ProductsReserveCancelSvcRequest
    {
        public ProductsReserveCancelSvcRequest(Guid orderID, uint userID)
        {
            OrderID = orderID;
            UserID = userID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
    }

    public class ProductsReserveCancelSvcResponse
    {
        public ProductsReserveCancelSvcResponse(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }
}
