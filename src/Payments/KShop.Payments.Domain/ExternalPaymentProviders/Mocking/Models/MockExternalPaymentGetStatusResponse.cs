using KShop.Shared.Domain.Contracts;

namespace KShop.Payments.Domain
{
    public class MockExternalPaymentGetStatusResponse : BaseResponse
    {
        public EMockPaymentStatus PaymentStatus { get; set; }
    }
}