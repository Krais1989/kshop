using KShop.Communications.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Invoices
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentPending_BusRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
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

    public class ExternalPaymentStatusChanged : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
        public EInvoiceStatus NewStatus { get; set; }
    }
}
