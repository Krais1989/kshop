using KShop.Communications.Contracts.Payments;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common.Models
{
    public class CommonPaymentProviderCancelRequest
    {
        public EPaymentProvider Provider { get; set; }
        public string ExternalPaymentID { get; set; }
    }
}