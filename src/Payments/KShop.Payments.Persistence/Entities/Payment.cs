﻿using KShop.Communications.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence.Entities
{
    public enum EPaymentStatus : int
    {
        /// <summary>
        /// Инициализация платежа (требуется инициализация во внешней системе)
        /// </summary>
        Initializing = 0,
        /// <summary>
        /// Ожидание платежа
        /// </summary>
        Pending,
        /// <summary>
        /// Оплачен
        /// </summary>
        Paid,
        /// <summary>
        /// Отмена протежа (требуется отмена во внешней системе)
        /// </summary>
        Cancelling,
        /// <summary>
        /// Отменён до обработки
        /// </summary>
        Canceled,
        /// <summary>
        /// Отклонено внешней системой
        /// </summary>
        Rejected,
        Error
    }

    public class Money 
    {
        public static class CurrencySign
        {
            public const string RUB = "RUB";
            public const string USD = "USD";
            public const string EUR = "EUR";
        }

        public Money(decimal amount, string currency = CurrencySign.RUB)
        {
            Currency = currency;
            Amount = amount;
        }

        public string Currency { get; private set; }
        public decimal Amount { get; private set; }

        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }

    public class Payment
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public string ExternalPaymentID { get; set; }
        public EPaymentProvider PaymentProvider { get; set; } // 
        public EPaymentStatus Status { get; set; }
        public DateTime StatusDate { get; set; }

        public Money Money { get; set; }

        public ICollection<PaymentLog> Logs { get; set; } = new List<PaymentLog>();

        public void SetStatus(EPaymentStatus newStatus, string logMessage = null)
        {
            Status = newStatus;
            StatusDate = DateTime.UtcNow;
            if (Logs == null) throw new Exception("Payments Logs not tracking! Cant add");
            Logs.Add(new PaymentLog() { ModifyDate = StatusDate, Status = Status, Message = logMessage });
        }
    }

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
