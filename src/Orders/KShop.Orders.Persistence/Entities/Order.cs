using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence.Entities
{
    public class Order
    {
        public enum EStatus : byte
        {
            Initial = 0,
            /// <summary>
            /// Резервирование, 
            /// </summary>
            Reserving = 1,
            /// <summary>
            /// Ожидание платежа
            /// </summary>
            Processing = 2,
            /// <summary>
            /// Доставляется покупателю
            /// </summary>
            Shipping = 3,
            /// <summary>
            /// Доставлен покупателю
            /// </summary>
            Completed = 4,
            /// <summary>
            /// Ошибка обработки
            /// </summary>
            Failed = 5,
            /// <summary>
            /// Возвращен
            /// </summary>
            Refunded = 6,
            /// <summary>
            /// Отменен покупателем
            /// </summary>
            Cancelled = 7
        }

        public Guid ID { get; set; }
        /// <summary>
        /// Получатель заказа
        /// </summary>
        public int CustomerID { get; set; }

        public EStatus Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime StatusDate { get; set; }
        public IEnumerable<OrderPosition> Positions { get; set; }
    }

    public class OrderPosition
    {
        public int ID { get; set; }
        public Guid OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }


        public Order Order { get; set; }
    }
}
