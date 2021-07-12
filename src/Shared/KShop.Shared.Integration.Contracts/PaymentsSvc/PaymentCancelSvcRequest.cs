using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Отмена платежа
    /// </summary>
    public class PaymentCancelSvcRequest
    {
        public PaymentCancelSvcRequest(Guid paymentID, uint userID)
        {
            UserID = userID;
            PaymentID = paymentID;
        }

        public uint UserID { get; private set; }
        public Guid PaymentID { get; private set; }
    }

    public class PaymentCancelSvcResponse : BaseResponse
    {
    }
}
