using System;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderPayFailureEvent
    {
        Guid OrderID { get; set; }
    }
    public class OrderPayFailureEvent : IOrderPayFailureEvent
    {
        public Guid OrderID { get; set; }
    }
}
