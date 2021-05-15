using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос на инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaRequest
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int, int> Positions { get; set; } = new Dictionary<int, int>();
    }

    /// <summary>
    /// Ответ на запрос инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaResponse
    {
        public Guid OrderID { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
