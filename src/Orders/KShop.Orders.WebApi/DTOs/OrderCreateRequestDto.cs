using KShop.Shared.Domain.Contracts;
using System.Collections.Generic;

namespace KShop.Orders.WebApi
{
    public class OrderCreateRequestDto
    {
        public string Address { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public OrderPositionsMap Positions { get; set; }
    }
}
