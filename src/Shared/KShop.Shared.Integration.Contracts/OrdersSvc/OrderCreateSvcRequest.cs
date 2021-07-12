using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Shared.Integration.Contracts
{

    /// <summary>
    /// Запрос к сервису Orders на создание записи заказа
    /// </summary>
    public class OrderCreateSvcRequest
    {
        public OrderCreateSvcRequest(Guid orderID, uint userID, List<ProductQuantity> orderContent, Money orderPrice)
        {
            OrderID = orderID;
            UserID = userID;
            OrderContent = orderContent;
            OrderPrice = orderPrice;
        }

        public Guid OrderID { get; private set; }
        public uint UserID { get; private set; }
        
        /* Данные продуктов для хранения в БД  */
        public List<ProductQuantity> OrderContent { get; private set; }

        public Money OrderPrice { get; private set; }
    }

    /// <summary>
    /// Событие шины об удачном создании записи заказа
    /// </summary>
    public class OrderCreateSuccessSvcEvent : BaseResponse
    {
        public OrderCreateSuccessSvcEvent(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }



    /// <summary>
    /// Событие шины об удачном создании записи заказа
    /// </summary>
    public class OrderCreateFaultSvcEvent
    {
        public OrderCreateFaultSvcEvent(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }
}
