using KShop.Shared.Domain.Contracts;

namespace KShop.Payments.Domain
{
    public class CommonPaymentProviderCancelRequest
    {
        public EPaymentProvider Provider { get; set; }
        public string ExternalPaymentID { get; set; }
    }
}