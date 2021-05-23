using System;

namespace KShop.Orders.Persistence.Entities
{
    public class OrderLog
    {
        public ulong ID { get; set; }
        public Guid OrderID { get; set; }

        public Order.EStatus NewStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string Message { get; set; }


        public Order Order { get; set; }
    }
}
