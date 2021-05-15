using System;

namespace KShop.Communications.Contracts.Orders
{
    /// <summary>
    /// Запрос на получение статуса
    /// </summary>
    public class OrderGetStatusSagaRequest : ICorrelationalMessage
    {
        public Guid CorrelationID { get; set; }
        public Guid OrderID { get; set; }
    }

    public class OrderGetStatusSagaResponse
    {
        public OrderGetStatusSagaResponse(int status = -1, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public int Status { get; set; } = -1;
        public bool IsSuccess => Status >= 0;
        public string ErrorMessage { get; private set; }
    }
}

