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

    public class OrderCancelSvcResponse : BaseResponse
    {
        public Guid? OrderID { get; set; }
    }
}
