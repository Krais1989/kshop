using KShop.Communications.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Invoices
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentCreateBusRequest : ICorrelationalMessage
    {
        public EPaymentPlatformType PaymentPlatform { get; set; }
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Отмена платежа
    /// </summary>
    public class PaymentCancelBusRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
    }


    public class PaymentProceedSuccess : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
    }

    public class PaymentProceedFailure : ICorrelationalMessage
    {
        public enum EReason
        {
            InternalError,
            Timeout
        }

        public Guid CorrelationID { get; set; }
        public EReason Reason { get; set; }
        public string Message { get; set; }
    }
}
