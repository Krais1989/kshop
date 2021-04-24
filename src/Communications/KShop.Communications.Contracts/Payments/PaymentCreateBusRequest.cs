using KShop.Communications.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Payments
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentCreateBusRequest
    {
        public EPaymentPlatformType PaymentPlatform { get; set; }
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentCreateBusResponse
    {
        public Guid? PaymentID { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }

    /// <summary>
    /// Отмена платежа
    /// </summary>
    public class PaymentCancelBusRequest
    {
        public Guid PaymentID { get; set; }
    }

    public class PaymentCancelBusResponse
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }

}
