namespace KShop.Products.Persistence.Entities
{
    public class ProductPosition
    {
        public uint ID { get; set; }
        public uint ProductID { get; set; }
        public uint Quantity { get; set; }


        public Product Product { get; set; }
    }
}
