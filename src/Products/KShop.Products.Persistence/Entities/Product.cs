using KShop.Shared.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
    public class Product
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
        public uint Discount { get; set; }
        public uint CategoryID { get; set; }
        public string Image { get; set; }
        

        public Category Category;
        public ICollection<ProductPosition> Positions { get; set; }
        public ICollection<ProductReserve> Reserves { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }

        public ICollection<ProductBookmark> ProductFavorits { get; set; }
    }

    public class ProductAttribute
    {
        public uint ProductID { get; set; }
        public uint AttributeID { get; set; }

        public string Value { get; set; }

        public Product Product { get; set; }
        public Attribute Attribute { get; set; }
    }

    public class Attribute
    {
        public uint ID { get; set; }
        public string Title { get; set; }

        public ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}
