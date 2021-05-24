using System;

namespace KShop.Communications.Contracts.Products
{
    public class ProductsReserveFaultEvent
    {
        public Guid OrderID { get; set; }
        public Exception Exception { get; set; }

        public ProductsReserveFaultEvent(Guid orderID, Exception exception = null)
        {
            OrderID = orderID;
            Exception = exception;
        }
    }
}
