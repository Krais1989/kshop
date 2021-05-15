using KShop.Communications.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Payments
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentCreateSvcRequest
    {
        public EPaymentPlatformType PaymentPlatform { get; set; }
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentCreateSvcResponse
    {
        public Guid? PaymentID { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
