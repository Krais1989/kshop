
using KShop.Shared.Domain.Contracts;

namespace KShop.Products.Domain
{
    public class ProductPresentation
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public uint CategoryID { get; set; }
        public Money Price { get; set; }

        public uint Discount { get; set; }
    }
}
