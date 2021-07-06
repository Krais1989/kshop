using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence
{
    public enum EOrderStatus : byte
    {
        None = 0,
        /// <summary>
        /// Товары зарезервированы
        /// </summary>
        Reserved = 1,
        /// <summary>
        /// Создан
        /// </summary>
        Created = 2,
        /// <summary>
        /// Оплачен
        /// </summary>
        Payed = 3,
        /// <summary>
        /// Доставлен
        /// </summary>
        Shipped = 4,
        /// <summary>
        /// Ошибка
        /// </summary>
        Faulted = 5,
        /// <summary>
        /// Возвращен
        /// </summary>
        Refunded = 6,
        /// <summary>
        /// Отменен покупателем
        /// </summary>
        Cancelled = 7
    }

    public class Order
    {
        public Guid ID { get; set; }
        /// <summary>
        /// Получатель заказа
        /// </summary>
        public uint CustomerID { get; set; }

        public Money Price { get; set; }

        public EOrderStatus Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime StatusDate { get; set; }
        public IEnumerable<OrderPosition> Positions { get; set; }
        public ICollection<OrderLog> Logs { get; set; }

        public void SetStatus(EOrderStatus newStatus, string logMessage = null)
        {
            if (Status != EOrderStatus.Created && Status == newStatus)
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
