
using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Products.Domain
{
    public class ProductDetails
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
    }

    public class ProductShort
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
    }
}
