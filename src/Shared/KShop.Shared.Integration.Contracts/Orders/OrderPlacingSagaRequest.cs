
using KShop.Shared.Domain.Contracts;
using System;
using System.Security.Claims;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{

    /// <summary>
    /// Запрос на инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaRequest
    {
        public uint Customer { get; set; }
        public Guid OrderID { get; set; }
        public OrderPositionsMap Positions { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        //public Money Price { get; set; }
    }

    /// <summary>
    /// Ответ на запрос инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaResponse
    {
        public Guid OrderID { get; set; }
    }

}
