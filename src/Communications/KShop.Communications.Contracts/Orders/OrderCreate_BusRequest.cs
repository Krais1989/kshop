using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    public class OrderCreate_BusRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int,int> Positions { get; set; }
    }

    public class OrderCreate_BusResponse : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid? OrderID { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    public class OrderCreateCompensate_BusRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
    }

    public class OrderCreateCompensate_BusResponse : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public bool IsSuccess { get; set; }
    }
}
