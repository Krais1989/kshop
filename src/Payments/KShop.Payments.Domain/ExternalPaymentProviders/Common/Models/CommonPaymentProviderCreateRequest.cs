using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Payments.Domain
{
    public class CommonPaymentProviderCreateRequest
    {
        public EPaymentProvider Provider { get; set; }
        public Guid OrderID { get; set; }
        public Money Money { get; set; }
    }
}