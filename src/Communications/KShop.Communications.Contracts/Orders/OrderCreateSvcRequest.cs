using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcRequest
    {
        public Guid? OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap Positions { get; set; }
    }

    /// <summary>
    /// Ответ на запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcResponse : BaseResponse
    {
        public Guid? OrderID { get; set; }
    }
}
