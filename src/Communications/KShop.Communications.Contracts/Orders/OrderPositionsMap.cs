using System.Collections.Generic;
using System.Linq;

namespace KShop.Communications.Contracts.Orders
{
    public class OrderPositionsMap : Dictionary<ulong, uint>
    {
        public OrderPositionsMap()
        {
        }

        public OrderPositionsMap(IDictionary<ulong, uint> dictionary) : base(dictionary)
        {
        }
    }

    /// <summary>
    /// Для десериализации
    /// </summary>
    public class OrderPositionsMapStr : Dictionary<string, uint>
    {
        public OrderPositionsMap Convert()
        {
            return new OrderPositionsMap(this.ToDictionary(e => ulong.Parse(e.Key), e => e.Value));
        }
    }
}
