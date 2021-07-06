using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Отмена платежа
    /// </summary>
    public class PaymentCancelSvcRequest
    {
        public Guid PaymentID { get; set; }
    }

    public class PaymentCancelSvcResponse : BaseResponse
    {
    }
}
