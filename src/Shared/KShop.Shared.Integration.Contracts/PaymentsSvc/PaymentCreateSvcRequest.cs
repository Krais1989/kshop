using KShop.Shared.Domain.Contracts;
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
        public PaymentCreateSvcRequest(Guid orderID, uint userID, EPaymentProvider paymentPlatform, Money money)
        {
            OrderID = orderID;
            UserID = userID;
            PaymentPlatform = paymentPlatform;
            Money = money;
        }

        public Guid OrderID { get; private set; }
        public uint UserID { get; private set; }
        public EPaymentProvider PaymentPlatform { get; private set; }
        public Money Money { get; private set; }
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
        public Guid OrderID { get; private set; }
        public Guid PaymentID { get; private set; }
    }

    public class PaymentCreateFaultSvcEvent
    {
        public PaymentCreateFaultSvcEvent(Guid orderID, string errorMessage = null)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }

        public Guid OrderID { get; private set; }
        public string ErrorMessage { get; private set; }

    }
}
