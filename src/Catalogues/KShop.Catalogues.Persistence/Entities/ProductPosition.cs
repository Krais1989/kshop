namespace KShop.Products.Persistence.Entities
{
    public class ProductPosition
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }


        public Product Product { get; set; }
    }
}
