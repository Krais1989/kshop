using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Products
{

    /// <summary>
    /// Событие для 
    /// </summary>
    public class ProductsReserve_BusEvent : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
        public IDictionary <int,int> Positions { get; set; }
    }
    public class ProductsReserveSuccess_BusEvent : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }

        public Guid ReserveID { get; set; }
    }
    public class ProductsReserveFailure_BusEvent : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
    }

    public class ProductsReserveCompensation_BusEvent : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid ReserveID { get; set; }
        public Guid OrderID { get; set; }
    }
}
