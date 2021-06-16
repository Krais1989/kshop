using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Domain
{
    public class OrderReserveException : Exception
    {
        public Guid OrderID { get; private set; }
        public OrderReserveException(Guid orderId, string message = "OrderReserveException") : base(message)
        {
            OrderID = orderId;
        }
    }
}
