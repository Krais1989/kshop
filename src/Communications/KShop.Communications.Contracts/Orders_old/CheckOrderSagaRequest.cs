using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders_old
{
    public interface ICheckOrderSagaRequest
    {
        Guid OrderID { get; }
        
    }

    public interface ICheckOrderSagaResponse
    {
        Guid OrderID { get; }
        int State { get; }
    }

    public interface IOrderNotFoundResponse
    {
        Guid OrderID { get; }
    }

    public class CheckOrderSagaRequest : ICheckOrderSagaRequest
    {
        public Guid OrderID { get; set; }
    }

    public class CheckOrderSagaResponse : ICheckOrderSagaResponse
    {
        public Guid OrderID { get; set; }
        public int State { get; set; }
    }
}
