using KShop.Shared.Domain.Contracts;

namespace KShop.Payments.Domain
{
    public class MockExternalPaymentCreateResponse : BaseResponse
    {
        public string ExternalPaymentID { get; set; }
    }
}