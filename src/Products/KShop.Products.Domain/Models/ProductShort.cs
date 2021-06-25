
using KShop.Shared.Domain.Contracts;

namespace KShop.Products.Domain
{
    public class ProductShort
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint CategoryID { get; set; }
        public Money Price { get; set; }
    }
}
