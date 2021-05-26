using System.Collections.Generic;

namespace KShop.Communications.Contracts.Products
{
    /// <summary>
    /// Показывает какие ID зарезервированных позиций соответствуют ID продукта
    /// [product_id, reserve_id]
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
