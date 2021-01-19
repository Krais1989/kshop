using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос на создание саги "Создание заказа"
    /// </summary>
    public class CreateOrderSaga_BusRequest
    {
        public int CustomerID { get; set; }
        public IDictionary<int, int> Positions { get; set; }
    }

    public class OrderCreate_BusEvent
    {
        public int CustomerID { get; set; }
        public IDictionary<int, int> Positions { get; set; }
    }

    public class OrderCreateSuccess_BusEvent
    {
        public Guid OrderID { get; set; }
    }

    public class OrderCreateFailure_BusEvent
    {
        public Guid OrderID { get; set; }
    }

    public class OrderCreateCompensation_BusEvent
    {
        public Guid OrderID { get; set; }
    }
}
