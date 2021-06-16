using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Событие успешного размещения заказа
    /// </summary>
    public class OrderPlacingSuccessSagaEvent
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap Positions { get; set; }
    }
}
