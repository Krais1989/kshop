using System;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderPaySuccessEvent
    {
        Guid OrderID { get; set; }
    }

    public class OrderPaySuccessEvent : IOrderPaySuccessEvent
    {
        public Guid OrderID { get; set; }
    }


}
