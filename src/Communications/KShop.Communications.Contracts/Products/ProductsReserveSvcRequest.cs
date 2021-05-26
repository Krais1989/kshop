using KShop.Communications.Contracts.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Products
{
    public class ProductsReserveSvcRequest
    {
        public Guid OrderID { get; set; }
        public ulong CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
    }

    public class ProductsReserveSvcResponse : BaseResponse
    {
        public ProductsReserveMap ProductsReserves { get; set; }
    }
}
