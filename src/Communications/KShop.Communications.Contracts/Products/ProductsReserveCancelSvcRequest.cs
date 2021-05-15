using System;

namespace KShop.Communications.Contracts.Products
{
    public class ProductsReserveCancelSvcRequest
    {
        public Guid ReserveID { get; set; }
    }

    public class ProductsReserveCancelSvcResponse
    {
        public Guid ReserveID { get; set; }
    }
}
