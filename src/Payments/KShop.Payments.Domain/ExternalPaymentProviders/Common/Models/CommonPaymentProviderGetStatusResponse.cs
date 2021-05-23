using KShop.Communications.Contracts;
using KShop.Payments.Persistence.Entities;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common.Models
{

    public class CommonPaymentProviderGetStatusResponse : BaseResponse
    {
        public EPaymentStatus PaymentStatus { get; set; }
    }
}