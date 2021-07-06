﻿using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Выставить чек
    /// </summary>
    public class PaymentCreateSvcRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public EPaymentProvider PaymentPlatform { get; set; }
        public Money Money { get; set; }
    }


    public class PaymentCreateSuccessSvcEvent
    {
        public PaymentCreateSuccessSvcEvent(Guid orderID, Guid paymentID)
        {
            OrderID = orderID;
            PaymentID = paymentID;
        }

        /// <summary>
        /// OrderID - для корреляции сообшений
        /// </summary>
        public Guid OrderID { get; set; }
        public Guid PaymentID { get; set; }
    }

    public class PaymentCreateFaultSvcEvent
    {
        public PaymentCreateFaultSvcEvent(Guid orderID, string errorMessage = null)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }

        public Guid OrderID { get; set; }
        public string ErrorMessage { get; set; }

    }
}