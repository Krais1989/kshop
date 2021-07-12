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
        public ProductsReserveSvcRequest(Guid orderID, uint userID, List<ProductQuantity> orderContent)
        {
            OrderID = orderID;
            UserID = userID;
            OrderContent = orderContent;
        }

        public Guid OrderID { get; private set; }
        public uint UserID { get; private set; }
        public List<ProductQuantity> OrderContent { get; private set; }
    }

    /// <summary>
    /// Сообщение внутреннего обработчика о состоянии резервации 
    /// </summary>
    public class ProductsReserveSuccessEvent
    {
        public Guid OrderID { get; private set; }
        public List<ProductQuantity> OrderContent { get; private set; }
        public Money Price { get; private set; }

        public ProductsReserveSuccessEvent(Guid orderID, List<ProductQuantity> orderContent, Money price)
        {
            OrderID = orderID;
            OrderContent = orderContent;
            Price = price;
        }
    }

    public class ProductsReserveFaultEvent : BaseResponse
    {
        public Guid OrderID { get; private set; }

        public ProductsReserveFaultEvent(Guid orderID)
        {
            OrderID = orderID;
        }

        public ProductsReserveFaultEvent(Guid orderID, string error) : base(error)
        {
            OrderID = orderID;
        }
    }
}
