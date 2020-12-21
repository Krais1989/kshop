using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence.Entities
{
    /*
        Pending — your payment has not yet been sent to the bank or credit card processor.

        Success — your credit or debit card payment has been processed and accepted.

        Complete — your checking, savings or other bank account payment has been processed and accepted.

        Canceled — you stopped the payment before it was processed. For automatic recurring payments, all remaining payments were canceled.

        Rejected — your payment was not accepted when it was processed by the bank or credit card company. For information, contact the bank or credit card company. Do not contact Pay.gov.
    */

    public class Payment
    {
        public enum EStatus
        {
            /// <summary>
            /// payment has not yet been sent to the bank or credit card processor.
            /// </summary>
            Pending,
            /// <summary>
            /// credit or debit card payment has been processed and accepted
            /// </summary>
            Success,
            /// <summary>
            /// checking, savings or other bank account payment has been processed and accepted
            /// </summary>
            Complete,
            /// <summary>
            /// stopped the payment before it was processed. For automatic recurring payments, all remaining payments were canceled
            /// </summary>
            Canceled,
            /// <summary>
            /// payment was not accepted when it was processed by the bank or credit card company. For information, contact the bank or credit card company
            /// </summary>
            Rejected
        }

        /// <summary>
        /// Соответствует ID заказа
        /// </summary>
        public Guid ID { get; set; }
        public EStatus Status { get; set; }
        public DateTime StatusDate { get; set; }

        public ICollection<PaymentLog> Logs { get; set; }
    }

    /// <summary>
    /// Запись лога изменения платежа
    /// </summary>
    public class PaymentLog
    {
        public int ID { get; set; }
        public Guid PaymentID { get; set; }
        public DateTime ModifyDate { get; set; }
        public Payment.EStatus Status { get; set; }

        public Payment Payment { get; set; }
    }
}
