using KShop.Communications.Contracts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain.ValueObjects
{
    public class ProductStack : IProductStack
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
