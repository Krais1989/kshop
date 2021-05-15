using System;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос для сервиса Orders на отмену заказа
    /// </summary>
    public class OrderCancelSvcRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderCancelSvcResponse
    {
        public Guid? OrderID { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
