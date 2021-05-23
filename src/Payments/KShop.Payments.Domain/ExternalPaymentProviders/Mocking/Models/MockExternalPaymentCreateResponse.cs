using KShop.Communications.Contracts;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Mocking.Models
{
    public class MockExternalPaymentCreateResponse : BaseResponse
    {
        public string ExternalPaymentID { get; set; }
    }
}