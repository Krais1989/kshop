using System;

namespace KShop.Payments.Persistence
{
    /// <summary>
    /// Запись лога изменения платежа
    /// </summary>
    public class PaymentLog
    {
        public int ID { get; set; }
        public Guid PaymentID { get; set; }
        public DateTime ModifyDate { get; set; }
        public EPaymentStatus Status { get; set; }
        public string Message { get; set; }

        public Payment Payment { get; set; }
    }
}
