using KShop.Shared.Domain.Contracts;
using System;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Запрос на получение статуса
    /// </summary>
    public class OrderGetStatusSagaRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderGetStatusSagaResponse : BaseResponse
    {
        public int Status { get; set; } = -1;
    }
}

