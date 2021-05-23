using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Сообщение создаваемое при завершении RS OrderPlacing
    /// </summary>
    public class OrderPlacingCompletedRSEvent
    {
        public Guid SubmissionID { get; set; }
        public DateTime TimeStamp { get; set; }


        public Guid OrderID { get; set; }
        public Guid ReserveID { get; set; }
    }

    /// <summary>
    /// Сообщение создаваемое при завершении RS OrderPlacing
    /// </summary>
    public class OrderPlacingFaultedRSEvent
    {
        public Guid SubmissionID { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
