using System.Collections.Generic;

namespace KShop.Products.Persistence
{
    public class Category
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
