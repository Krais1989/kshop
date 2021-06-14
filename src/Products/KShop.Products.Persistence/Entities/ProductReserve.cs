using System;

namespace KShop.Products.Persistence.Entities
{
    public class ProductReserve
    {
        public enum EStatus : byte
        {
            Pending,
            Reserved,
            Success,
            Failure,
            NotEnough
        }

        public uint ID { get; set; }
        public uint ProductID { get; set; }
        public uint Quantity { get; set; }

        public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }

        /// <summary>
        /// Дата создания резерва
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Дата завершения резерва (удачное, отмена, ошибка)
        /// </summary>
        public DateTime CompleteDate { get; set; }

        public EStatus Status { get; set; }

        public Product Product { get; set; }
    }
}
