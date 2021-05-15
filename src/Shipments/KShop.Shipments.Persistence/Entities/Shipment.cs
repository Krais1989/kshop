using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Persistence.Entities
{
    public enum EShipmentStatus
    {
        Initializing,
        Pending,
        Shipped,
        Cancelling,
        Cancelled,
        Faulted
    }

    public class Shipment
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Дата начала обработки
        /// </summary>
        public DateTime PendingDate { get; set; }
        /// <summary>
        /// Дата завершения (успешно или с ошибкой)
        /// </summary>
        public DateTime CompleteDate { get; set; }

        /// <summary>
        /// ID во внешней системе
        /// </summary>
        public string ExternalID { get; set; }

        public EShipmentStatus Status { get; set; }
    }
}
