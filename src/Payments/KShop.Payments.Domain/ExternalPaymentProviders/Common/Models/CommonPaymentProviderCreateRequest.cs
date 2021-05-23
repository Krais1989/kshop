using KShop.Communications.Contracts.Payments;
using System;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common.Models
{
    public class CommonPaymentProviderCreateRequest
    {
        public EPaymentProvider Provider { get; set; }
        public Guid OrderID { get; set; }
    }
}