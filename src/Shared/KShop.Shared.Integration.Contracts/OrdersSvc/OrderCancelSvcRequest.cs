using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    public class OrderCancelSvcRequest
    {
        public OrderCancelSvcRequest(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; set; }
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
