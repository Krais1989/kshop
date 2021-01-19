using System;

namespace KShop.Communications.Contracts.Orders_old
{
    public interface IOrderPayEvent
    {
        Guid OrderID { get; set; }
        Decimal Price { get; set; }
    }

    public class OrderPayEvent : IOrderPayEvent
    {
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }




}
