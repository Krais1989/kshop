using KShop.Communications.Contracts.ValueObjects;
using System;
using System.Collections.Generic;

namespace KShop.Communications.Contracts.Orders
{
    public interface IOrderReserveEvent
    {
        Guid OrderID { get; set; }
        IEnumerable<IProductStack> Positions { get; set; }
    }

    public class OrderReserveEvent : IOrderReserveEvent
    {
        public Guid OrderID { get; set; }
        public IEnumerable<IProductStack> Positions { get; set; }
    }



}
