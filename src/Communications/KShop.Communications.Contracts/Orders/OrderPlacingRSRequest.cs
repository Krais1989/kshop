using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Событие для инициализации RS OrderPlacing
    /// </summary>
    public class OrderPlacingRSRequest
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public Money Price { get; set; }
    }
    //public class OrderPlacingRSResponse
    //{
    //    public string Data { get; set; }
    //}
    //public class OrderPlacingSuccessRSEvent
    //{
    //    public string SuccessMessage { get; set; }
    //}
    //public class OrderPlacingFailureRSEvent
    //{
    //    public string ErrorMessage { get; set; }
    //}

}
