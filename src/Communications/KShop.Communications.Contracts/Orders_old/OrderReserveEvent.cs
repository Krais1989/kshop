using KShop.Communications.Contracts.ValueObjects;
using System;
using System.Collections.Generic;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderReserveEvent
    {
        Guid OrderID { get; set; }
        IEnumerable<ProductStack> Positions { get; set; }
    }

    public class OrderReserveEvent : IOrderReserveEvent
    {
        public Guid OrderID { get; set; }
        public IEnumerable<ProductStack> Positions { get; set; }
    }



}
