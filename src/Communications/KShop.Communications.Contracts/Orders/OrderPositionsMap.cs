using System.Collections.Generic;
using System.Linq;

namespace KShop.Communications.Contracts.Orders
{
    public class OrderPositionsMap : Dictionary<uint, uint>
    {
        public OrderPositionsMap()
        {
        }

        public OrderPositionsMap(IDictionary<uint, uint> dictionary) : base(dictionary)
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
            return new OrderPositionsMap(this.ToDictionary(e => uint.Parse(e.Key), e => e.Value));
        }
    }
}
