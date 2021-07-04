using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcRequest
    {
        public Guid? OrderID { get; set; }
        public uint CustomerID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
    }

    /// <summary>
    /// Ответ на запрос для сервиса Orders на создание заказа
    /// </summary>
    public class OrderCreateSvcResponse : BaseResponse
    {
        public Guid? OrderID { get; set; }
    }
}
