using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Persistence
{
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

        public DateTime LastCheckingDate { get; set; }

        /// <summary>
        /// ID во внешней системе
        /// </summary>
        public string ExternalID { get; set; }

        public EShipmentStatus Status { get; set; }

        public void SetStatus(EShipmentStatus newStatus)
        {
            Status = newStatus;
            switch (Status)
            {
                case EShipmentStatus.Initializing:
                    CreateDate = DateTime.UtcNow;
                    break;
                case EShipmentStatus.Pending:
                    PendingDate = DateTime.UtcNow;
                    break;
                case EShipmentStatus.Shipped:
                    CompleteDate = DateTime.UtcNow;
                    break;
                case EShipmentStatus.Cancelling:
                    break;
                case EShipmentStatus.Cancelled:
                    CompleteDate = DateTime.UtcNow;
                    break;
                case EShipmentStatus.Error:
                    CompleteDate = DateTime.UtcNow;
                    break;
            }
        }
    }
}
