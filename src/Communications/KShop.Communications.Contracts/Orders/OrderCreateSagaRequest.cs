using KShop.Communications.Contracts.ValueObjects;
using System;
using System.Collections.Generic;

namespace KShop.Communications.Contracts.Orders
{

    /// <summary>
    /// Событие для создания заказа
    /// </summary>
    public interface IOrderCreateSagaRequest
    {
        Guid OrderID { get; set; }
        IEnumerable<IProductStack> Positions { get; set; }
    }

    public class OrderCreateSagaRequest : IOrderCreateSagaRequest
    {
        public Guid OrderID { get; set; }
        public IEnumerable<IProductStack> Positions { get; set; }
    }

    public interface IOrderCreateSagaResponse
    {
        Guid OrderID { get; set; }
    }
    public class OrderCreateSagaResponse : IOrderCreateSagaResponse
    {
        public Guid OrderID { get; set; }
    }
}
