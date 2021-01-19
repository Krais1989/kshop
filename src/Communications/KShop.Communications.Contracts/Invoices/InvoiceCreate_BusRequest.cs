using KShop.Communications.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Invoices
{
    public class InvoiceCreate_BusRequest
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal Price { get; set; }
    }

    public class InvoiceCreate_BusResponse
    {
        public InvoiceCreate_BusResponse(bool isSuccess = true, string errorMessage = "")
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
    }

    public class InvoiceStatusChanged_BusEvent
    {
        public Guid OrderID { get; set; }
        public EInvoiceStatus InvoiceStatus { get; set; }
    }
}
