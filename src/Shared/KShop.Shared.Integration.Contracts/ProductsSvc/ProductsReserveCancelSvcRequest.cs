using System;

namespace KShop.Shared.Integration.Contracts
{
    public class ProductsReserveCancelSvcRequest
    {
        public ProductsReserveCancelSvcRequest(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; set; }
    }

    public class ProductsReserveCancelSvcResponse
    {
        public ProductsReserveCancelSvcResponse(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; set; }
    }
}
