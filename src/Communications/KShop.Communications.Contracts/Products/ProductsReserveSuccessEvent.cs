using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Communications.Contracts.Products
{

    /// <summary>
    /// Сообщение внутреннего обработчика о состоянии резервации 
    /// </summary>
    public class ProductsReserveSuccessEvent
    {
        public Guid OrderID { get; set; }
        /// <summary>
        /// Зарезервированные продукты <product_id, quantity>
        /// </summary>
        public ProductsReserveMap ReservedProducts { get; set; }

        public ProductsReserveSuccessEvent(Guid orderID, ProductsReserveMap reservedProducts)
        {
            OrderID = orderID;
            ReservedProducts = reservedProducts;
        }
    }
}
