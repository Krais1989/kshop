using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Products
{
    public class ProductsReserveSvcRequest
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int, int> OrderPositions { get; set; }
    }

    public class ProductsReserveSvcResponse : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid? ReserveID { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
