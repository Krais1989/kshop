using System;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderReserveFailureEvent
    {
        Guid OrderID { get; set; }
    }

    public class OrderReserveFailureEvent : IOrderReserveFailureEvent
    {
        public Guid OrderID { get; set; }
    }
}
