using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Payments
{
    public class PaymentProcessingSMRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }

    }
}
