using System;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос для сервиса Orders на отмену заказа
    /// </summary>
    public class OrderCancelSvcRequest
    {
        public OrderCancelSvcRequest(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; set; }
    }

    public class OrderCancelSvcResponse : BaseResponse
    {
        public OrderCancelSvcResponse(Guid? orderID)
        {
            OrderID = orderID;
        }

        public Guid? OrderID { get; set; }
    }
}
