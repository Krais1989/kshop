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
        int CustomerID { get; set; }
        IEnumerable<ProductStack> Positions { get; set; }
    }

    public class OrderCreateSagaRequest : IOrderCreateSagaRequest
    {
        public int CustomerID { get; set; }
        public IEnumerable<ProductStack> Positions { get; set; }
    }

    public interface IOrderCreateSagaResponse
    {
        Guid OrderID { get; set; }
        bool IsSuccess { get; set; }
    }

    public class OrderCreateSagaResponse : IOrderCreateSagaResponse
    {
        public Guid OrderID { get; set; }
        public bool IsSuccess { get; set; }
    }
}
