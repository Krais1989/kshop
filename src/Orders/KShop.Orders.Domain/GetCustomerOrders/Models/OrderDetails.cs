using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderDetails
    {
        public class Log
        {
            public EOrderStatus Status { get; set; }
            public DateTime Date { get; set; }
        }

        public class Position
        {
            public uint ProductID { get; set; }
            public uint Quantity { get; set; }
        }

        public Guid ID { get; set; }
        public EOrderStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public Money Price { get; set; }

        public IEnumerable<Log> Logs { get; set; }
        public IEnumerable<Position> Positions { get; set; }
    }
}
