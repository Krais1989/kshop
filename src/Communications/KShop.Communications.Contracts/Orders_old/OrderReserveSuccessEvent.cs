﻿using System;

namespace KShop.Communications.Contracts.Orders_old
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
