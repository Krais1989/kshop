using System;

namespace KShop.Products.Persistence
{
    public class ProductPosition
    {
        public uint ID { get; set; }
        public uint ProductID { get; set; }
        public uint Quantity { get; set; }

        public DateTime CreateDate { get; set; }


        public Product Product { get; set; }
    }
}
