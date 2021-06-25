﻿
using KShop.Shared.Domain.Contracts;
using System;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{

    /// <summary>
    /// Запрос на инициализации саги создания заказа
    /// </summary>
    public class OrderPlacingSagaRequest
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap Positions { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        //public Money Price { get; set; }
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