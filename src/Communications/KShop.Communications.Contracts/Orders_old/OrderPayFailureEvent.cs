using System;

namespace KShop.Communications.Contracts.Orders_old
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
