using System;
using System.Collections.Generic;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Событие успешного размещения заказа
    /// </summary>
    public class OrderPlacingSuccessSagaEvent
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int, int> Positions { get; set; } = new Dictionary<int, int>();
    }
}
