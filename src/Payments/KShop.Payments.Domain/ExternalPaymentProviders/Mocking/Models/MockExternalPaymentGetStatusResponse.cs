using KShop.Communications.Contracts;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Mocking.Models
{
    public class MockExternalPaymentGetStatusResponse : BaseResponse
    {
        public EMockPaymentStatus PaymentStatus { get; set; }
    }
}