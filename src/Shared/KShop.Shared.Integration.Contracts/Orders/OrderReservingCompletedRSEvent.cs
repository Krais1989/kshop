using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Сообщение создаваемое при завершении RS OrderPlacing
    /// </summary>
    public class OrderReservingCompletedRSEvent
    {
        public Guid OrderID { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid ReserveID { get; set; }
    }

    ///// <summary>
    ///// Сообщение создаваемое при завершении RS OrderPlacing
    ///// </summary>
    //public class OrderReservingFaultedRSEvent
    //{
    //    public Guid OrderID { get; set; }
    //    public DateTime TimeStamp { get; set; }
    //}
}
