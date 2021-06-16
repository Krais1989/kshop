using System;

namespace KShop.Shared.Integration.Contracts
{
    public class ProductsReserveFaultEvent
    {
        public Guid OrderID { get; set; }
        public string ErrorMessage { get; set; }

        public ProductsReserveFaultEvent(Guid orderID, string errorMessage = null)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }
    }
}
