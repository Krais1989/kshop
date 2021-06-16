using System;

namespace KShop.Orders.Persistence
{
    public class OrderPosition
    {
        public uint ID { get; set; }
        public Guid OrderID { get; set; }
        public uint ProductID { get; set; }
        public uint Quantity { get; set; }


        public Order Order { get; set; }
    }
}
