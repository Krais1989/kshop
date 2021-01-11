using System;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderReserveCompensationEvent
    {
        Guid OrderID { get; set; }
    }

    public class OrderReserveCompensationEvent : IOrderReserveCompensationEvent
    {
        public Guid OrderID { get; set; }
    }



}
