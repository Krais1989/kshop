using KShop.Communications.Contracts;
using System;

namespace KShop.Payments.Domain.ExternalPaymentProviders.Common.Models
{
    public class CommonPaymentProviderCreateResponse : BaseResponse
    {
        public string ExternalPaymentID { get; set; }
    }
}