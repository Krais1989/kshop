﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Payments
{
    /// <summary>
    /// Событие выбрасывается оркестратором при отмене/ошибке оплаты
    /// </summary>
    public class PaymentPendingCancelledSagaEvent
    {
        public Guid OrderID { get; set; }
        public string Message { get; set; }
    }
}
