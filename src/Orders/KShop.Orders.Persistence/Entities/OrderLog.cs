using System;

namespace KShop.Orders.Persistence
{
    public class OrderLog
    {
        public uint ID { get; set; }
        public Guid OrderID { get; set; }

        public EOrderStatus NewStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string Message { get; set; }


        public Order Order { get; set; }
    }
}
