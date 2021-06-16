using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence
{
    public class Payment
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public string ExternalID { get; set; }
        public EPaymentProvider PaymentProvider { get; set; } // 
        public EPaymentStatus Status { get; set; }
        public DateTime StatusDate { get; set; }

        public DateTime LastCheckingDate { get; set; }

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
}
