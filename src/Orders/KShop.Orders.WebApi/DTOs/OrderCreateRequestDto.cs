using KShop.Shared.Domain.Contracts;
using System.Collections.Generic;

namespace KShop.Orders.WebApi
{
    public class OrderCreateRequestDto
    {
        public OrderPositionsMap Positions { get; set; }
    }
}
