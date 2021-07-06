using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.Contracts
{
    /// <summary>
    /// Запрос к сервису Products на резервацию товаров
    /// </summary>
    public class ProductsReserveSvcRequest
    {
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
    }

    //public class ProductsReserveSvcResponse : BaseResponse
    //{
    //    public ProductsReserveMap ProductsReserves { get; set; }
    //    public Money OrderPrice { get; set; }
    //}

    /// <summary>
    /// Сообщение внутреннего обработчика о состоянии резервации 
    /// </summary>
    public class ProductsReserveSuccessEvent
    {
        public Guid OrderID { get; set; }
        /// <summary>
        /// Зарезервированные продукты <product_id, quantity>
        /// </summary>
        public List<ProductQuantity> OrderContent { get; set; }

        public Money Price { get; set; }

        public ProductsReserveSuccessEvent(Guid orderID, List<ProductQuantity> orderContent, Money price)
        {
            OrderID = orderID;
            OrderContent = orderContent;
            Price = price;
        }
    }

    public class ProductsReserveFaultEvent
    {
        public Guid OrderID { get; set; }
        public string ErrorMessage { get; set; }

        public ProductsReserveFaultEvent(Guid orderID, string errorMessage = null)
        {
            OrderID = orderID;
            ErrorMessage = errorMessage;
        }
    }
}
