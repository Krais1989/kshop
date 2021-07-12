
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
        public OrderSubmitSagaRequest(
            Guid orderID,
            uint customerID,
            List<ProductQuantity> orderContent,
            EPaymentProvider paymentProvider,
            EShippingMethod shippingMethod,
            Address address)
        {
            OrderID = orderID;
            UserID = customerID;
            OrderContent = orderContent;
            PaymentProvider = paymentProvider;
            ShippingMethod = shippingMethod;
            Address = address;
        }

        public Guid OrderID { get; private set; }
        public uint UserID { get; private set; }
        public List<ProductQuantity> OrderContent { get; private set; }
        public EPaymentProvider PaymentProvider { get; private set; }
        public EShippingMethod ShippingMethod { get; private set; }
        public Address Address { get; private set; }
        //public Money Price { get; set; }
    }



    /// <summary>
    /// Ответ на запрос инициализации саги создания заказа
    /// </summary>
    public class OrderSubmitSagaResponse : BaseResponse
    {
        public OrderSubmitSagaResponse(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }

}
