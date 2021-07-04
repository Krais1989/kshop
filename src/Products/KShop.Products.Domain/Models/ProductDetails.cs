
using KShop.Products.Persistence;
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
        public class Attribute
        {
            public uint ID { get; set; }
            public string Title { get; set; }
            public string Value { get; set; }
        }

        public uint ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Money Price { get; set; }
        public uint Discount { get; set; }
        public string Image { get; set; }

        public ICollection<Attribute> Attributes { get; set; }
    }

    //public class ProductDetailsAttribute
    //{
    //    public uint ID { get; set; }
    //    public string Title { get; set; }
    //    public string Value { get; set; }
    //}
}
