using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence
{
    public enum EOrderStatus : byte
    {
        Initialized = 0,
        /// <summary>
        /// Резервирование, 
        /// </summary>
        Reserved = 1,
        /// <summary>
        /// Ожидание платежа
        /// </summary>
        Payed = 2,
        /// <summary>
        /// Доставляется покупателю
        /// </summary>
        Shipped = 3,
        /// <summary>
        /// Ошибка обработки
        /// </summary>
        Faulted = 4,
        /// <summary>
        /// Возвращен
        /// </summary>
        Refunded = 5,
        /// <summary>
        /// Отменен покупателем
        /// </summary>
        Cancelled = 6
    }

    public class Order
    {
        public Guid ID { get; set; }
        /// <summary>
        /// Получатель заказа
        /// </summary>
        public uint CustomerID { get; set; }

        public EOrderStatus Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime StatusDate { get; set; }
        public IEnumerable<OrderPosition> Positions { get; set; }
        public ICollection<OrderLog> Logs { get; set; }

        public void SetStatus(EOrderStatus newStatus, string logMessage = null)
        {
            if (Status != EOrderStatus.Initialized && Status == newStatus)
            {
                throw new Exception($"Exception while changing to same status ({newStatus})! OrderID: {ID}");
            }

            Status = newStatus;
            StatusDate = DateTime.UtcNow;
            if (Logs == null) // throw new Exception($"Order Logs not tracking! {ID}");
                Logs = new List<OrderLog>();

            Logs.Add(new OrderLog()
            {
                StatusDate = StatusDate,
                NewStatus = Status,
                Message = logMessage
            });
        }
    }
}
