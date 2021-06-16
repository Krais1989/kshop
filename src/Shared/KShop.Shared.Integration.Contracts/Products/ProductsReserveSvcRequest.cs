using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    public class ProductsReserveSvcRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
    }

    public class ProductsReserveSvcResponse : BaseResponse
    {
        public ProductsReserveMap ProductsReserves { get; set; }
    }
}
