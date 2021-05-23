using System;

namespace KShop.Orders.Persistence.Entities
{
    public class OrderPosition
    {
        public ulong ID { get; set; }
        public Guid OrderID { get; set; }
        public ulong ProductID { get; set; }
        public uint Quantity { get; set; }


        public Order Order { get; set; }
    }
}
