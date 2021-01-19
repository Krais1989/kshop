using System;

namespace KShop.Products.Persistence.Entities
{
    public class ProductReserve
    {
        public enum EStatus : byte
        {
            Reserving,
            Reserved,
            Success,
            Failure
        }

        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public Guid OrderID { get; set; }

        public DateTime ReserveDate { get; set; }
        public EStatus Status { get; set; }


        public Product Product { get; set; }
    }
}
