using KShop.Communications.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Invoices
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentPending
    {

    }

    /// <summary>
    /// Успешное выставление чека
    /// </summary>
    public class PaymentPending_Success
    {

    }

    /// <summary>
    /// l 
    /// </summary>
    public class PaymentProceed_Success
    {

    }

    /// <summary>
    ///
    /// </summary>
    public class PaymentProceed_Failure
    {
        public enum EPaymentFailureReason : byte
        {
            InternalError = 0, 
            Timeout
        }

        public EPaymentFailureReason Reason { get; private set; }
    }
}
