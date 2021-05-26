using System;

namespace KShop.Communications.Contracts.Orders
{
    //public enum EStatus : byte
    //{
    //    Initial = 0,
    //    /// <summary>
    //    /// Резервирование, 
    //    /// </summary>
    //    Reserving = 1,
    //    /// <summary>
    //    /// Ожидание платежа
    //    /// </summary>
    //    Processing = 2,
    //    /// <summary>
    //    /// Доставляется покупателю
    //    /// </summary>
    //    Shipping = 3,
    //    /// <summary>
    //    /// Доставлен покупателю
    //    /// </summary>
    //    Completed = 4,
    //    /// <summary>
    //    /// Ошибка обработки
    //    /// </summary>
    //    Failed = 5,
    //    /// <summary>
    //    /// Возвращен
    //    /// </summary>
    //    Refunded = 6,
    //    /// <summary>
    //    /// Отменен покупателем
    //    /// </summary>
    //    Cancelled = 7
    //}

    public class OrderSetStatusReservedSvcRequest
    {
        public OrderSetStatusReservedSvcRequest()
        {
        }

        public OrderSetStatusReservedSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }

    public class OrderSetStatusPayedSvcRequest
    {
        public OrderSetStatusPayedSvcRequest()
        {
        }

        public OrderSetStatusPayedSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }

    public class OrderSetStatusShippedSvcRequest
    {
        public OrderSetStatusShippedSvcRequest()
        {
        }

        public OrderSetStatusShippedSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }

    public class OrderSetStatusFaultedSvcRequest
    {
        public OrderSetStatusFaultedSvcRequest()
        {
        }

        public OrderSetStatusFaultedSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }

    public class OrderSetStatusRefundedSvcRequest
    {
        public OrderSetStatusRefundedSvcRequest()
        {
        }

        public OrderSetStatusRefundedSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }
    public class OrderSetStatusCancelledSvcRequest
    {
        public OrderSetStatusCancelledSvcRequest()
        {
        }

        public OrderSetStatusCancelledSvcRequest(Guid orderID, string comment = null)
        {
            OrderID = orderID;
            Comment = comment;
        }

        public Guid OrderID { get; set; }
        public string Comment { get; set; }
    }


    public class OrderSetStatusSvcResponse : BaseResponse
    {
    }

}

