using KShop.Shared.Domain.Contracts;

namespace KShop.Payments.Domain
{
    public class CommonPaymentProviderGetStatusRequest
    {
        public EPaymentProvider Provider { get; set; }
        public string ExternalPaymentID { get; set; }
    }
}