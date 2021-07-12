using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Запрос на получение статуса
    /// </summary>
    public class OrderGetStatusSagaRequest
    {
        public OrderGetStatusSagaRequest(Guid orderID, uint userID)
        {
            OrderID = orderID;
            UserID = userID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
    }

    public class OrderGetStatusSagaResponse : BaseResponse
    {
        public OrderGetStatusSagaResponse(int status)
        {
            Status = status;
        }

        public int Status { get; private set; } = -1;
    }
}

