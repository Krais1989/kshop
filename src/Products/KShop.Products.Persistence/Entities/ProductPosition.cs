namespace KShop.Products.Persistence.Entities
{
    public class ProductPosition
    {
        public ulong ID { get; set; }
        public ulong ProductID { get; set; }
        public uint Quantity { get; set; }


        public Product Product { get; set; }
    }
}
