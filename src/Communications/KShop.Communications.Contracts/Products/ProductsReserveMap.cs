using System.Collections.Generic;

namespace KShop.Communications.Contracts.Products
{
    /// <summary>
    /// Показывает какие ID зарезервированных позиций соответствуют ID продукта
    /// [product_id, reserve_id]
    /// </summary>
    public class ProductsReserveMap : Dictionary<uint, uint>
    {
        public ProductsReserveMap()
        {
        }

        public ProductsReserveMap(IDictionary<uint, uint> dictionary) : base(dictionary)
        {
        }
    }
}
