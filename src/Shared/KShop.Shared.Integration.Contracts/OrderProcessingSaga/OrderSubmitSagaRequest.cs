
using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Запрос на инициализации саги создания заказа
    /// </summary>
    public class OrderSubmitSagaRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public Address Address { get; set; }
        //public Money Price { get; set; }
    }



    /// <summary>
    /// Ответ на запрос инициализации саги создания заказа
    /// </summary>
    public class OrderSubmitSagaResponse : BaseResponse
    {
        public Guid OrderID { get; set; }
    }

}
