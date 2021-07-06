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
        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        
        /* Данные продуктов для хранения в БД  */
        public List<ProductQuantity> OrderContent { get; set; }

        public Money OrderPrice { get; set; }
    }

    /// <summary>
    /// Событие шины об удачном создании записи заказа
    /// </summary>
    public class OrderCreateSuccessSvcEvent : BaseResponse
    {
        public Guid OrderID { get; set; }
    }



    /// <summary>
    /// Событие шины об удачном создании записи заказа
    /// </summary>
    public class OrderCreateFaultSvcEvent
    {
        public Guid OrderID { get; set; }
    }
}
