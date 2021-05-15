using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int,int> Positions { get; set; }
    }

    /// <summary>
    /// Ответ на запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcResponse : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid? OrderID { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
