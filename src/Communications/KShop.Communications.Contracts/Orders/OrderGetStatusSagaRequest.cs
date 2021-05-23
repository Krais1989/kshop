using System;

namespace KShop.Communications.Contracts.Orders
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

