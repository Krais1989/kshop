using System.Collections.Generic;

namespace KShop.Communications.Contracts.Products
{
    /// <summary>
    /// [product_id, quantity]
    /// </summary>
    public class ProductsReserveMap : Dictionary<ulong, ulong>
    {
        public ProductsReserveMap()
        {
        }

        public ProductsReserveMap(IDictionary<ulong, ulong> dictionary) : base(dictionary)
        {
        }
    }
}
