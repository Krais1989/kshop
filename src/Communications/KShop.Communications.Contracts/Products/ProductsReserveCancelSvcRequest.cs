using System;

namespace KShop.Communications.Contracts.Products
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
