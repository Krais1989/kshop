using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Products
{
    /// <summary>
    /// Сообщение внутреннего обработчика о состоянии резервации 
    /// </summary>
    public class ProductsReserveBSEvent
    {
        public Guid OrderID { get; set; }
        /// <summary>
        /// Зарезервированные продукты <product_id, quantity>
        /// </summary>
        public IDictionary<ulong, uint> ReservedProducts { get; set; }
    }
}
