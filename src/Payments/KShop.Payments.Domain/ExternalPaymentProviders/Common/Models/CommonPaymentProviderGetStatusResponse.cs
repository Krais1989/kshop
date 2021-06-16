using KShop.Payments.Persistence;
using KShop.Shared.Domain.Contracts;


namespace KShop.Payments.Domain
{

    public class CommonPaymentProviderGetStatusResponse : BaseResponse
    {
        public EPaymentStatus PaymentStatus { get; set; }
    }
}