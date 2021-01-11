using System;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Событие для отмены заказа
    /// </summary>
    public interface IOrderCancelEvent
    {
        Guid OrderID { get; set; }
    }


    /// <summary>
    /// Событие для отмены заказа
    /// </summary>
    public class OrderCancelEvent : IOrderCancelEvent
    {
        public Guid OrderID { get; set; }
    }

}
