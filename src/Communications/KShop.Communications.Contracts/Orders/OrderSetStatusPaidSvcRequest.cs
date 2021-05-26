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

    public class OrderSetStatusPaidSvcRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderSetStatusErrorSvcRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderSetStatusCanceledSvcRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderSetStatusReservedSvcRequest
    {
        public Guid OrderID { get; set; }
    }
}

