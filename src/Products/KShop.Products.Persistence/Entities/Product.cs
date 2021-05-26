using KShop.Communications.Contracts.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence.Entities
{
    public class Product
    {
        public ulong ID { get; set; }
        public string Title { get; set; }
        public Money Money { get; set; }
        
        public ICollection<ProductPosition> Positions { get; set; }
        public ICollection<ProductReserve> Reserves { get; set; }
    }
}
