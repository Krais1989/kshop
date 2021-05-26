using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.ValueObjects;
using System;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common.Models
{
    public class CommonPaymentProviderCreateRequest
    {
        public EPaymentProvider Provider { get; set; }
        public Guid OrderID { get; set; }
        public Money Money { get; set; }
    }
}