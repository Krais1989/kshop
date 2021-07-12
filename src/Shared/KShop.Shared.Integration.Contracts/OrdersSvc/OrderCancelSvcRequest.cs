using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    public class OrderCancelSvcRequest
    {
        public OrderCancelSvcRequest(Guid orderID, uint userID)
        {
            OrderID = orderID;
            UserID = userID;
        }

        public Guid OrderID { get; private set; }
        public uint UserID { get; private set; }
    }

    public class OrderCancelSvcResponse : BaseResponse
    {
        public OrderCancelSvcResponse(bool isSuccess = true) : base(isSuccess)
        {
        }

        public OrderCancelSvcResponse(string error) : base(error)
        {
        }
    }
}
