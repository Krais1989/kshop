using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Orders
{
    public class OrderCreate_SagaRequest
    {
        public int CustomerID { get; set; }
        public IDictionary<int, int> Positions { get; set; }
    }

    public class OrderCreate_SagaResponse
    {
        public Guid OrderID { get; set; }
    }

    public class OrderGetStatus_SagaRequest
    {
        public Guid OrderID { get; set; }
    }

    public class OrderGetStatus_SagaResponse
    {
        public OrderGetStatus_SagaResponse(int status = -1, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public int Status { get; set; } = -1;
        public bool IsSuccess => Status >= 0;
        public string ErrorMessage { get; private set; }
    }
}
