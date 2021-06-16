using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Событие выбрасывается оркестратором при отмене/ошибке доставки
    /// </summary>
    public class ShipmentPendingCancelledSagaEvent
    {
        public Guid OrderID { get; set; }
        public string Message { get; set; }
    }
}
