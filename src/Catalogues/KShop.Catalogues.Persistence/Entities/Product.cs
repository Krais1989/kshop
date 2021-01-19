using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductPosition> Positions { get; set; }
        public ICollection<ProductReserve> Reserves { get; set; }
    }
}
