using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Payments.Domain
{
    public class CommonPaymentProviderCreateResponse : BaseResponse
    {
        public string ExternalPaymentID { get; set; }
    }
}