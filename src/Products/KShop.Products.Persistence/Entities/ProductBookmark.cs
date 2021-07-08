using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
    public class ProductBookmark
    {
        public uint ProductID { get; set; }

        public uint CustomerID { get; set; }

        public Product Product { get; set; }
    }
}
