using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    public class PaymentProcessingSMRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }

    }
}
