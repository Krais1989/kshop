using System;
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
        public OrderPositionsMap Positions { get; set; }
    }

    /// <summary>
    /// Ответ на запрос инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
