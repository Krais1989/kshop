using System;

namespace KShop.Communications.Contracts.Payments
{
    /// <summary>
    /// Отмена платежа
    /// </summary>
    public class PaymentCancelSvcRequest
    {
        public Guid PaymentID { get; set; }
    }

    public class PaymentCancelSvcResponse
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
