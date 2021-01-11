using System;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderReserveSuccessEvent
    {
        Guid OrderID { get; set; }
    }

    public class OrderReserveSuccessEvent : IOrderReserveSuccessEvent
    {
        public Guid OrderID { get; set; }
    }

}
