using KShop.Communications.Contracts.Orders;
using System.Collections.Generic;

namespace KShop.Orders.WebApi.DTOs
{
    public class OrderCreateRequestDto
    {
        public OrderPositionsMap Positions { get; set; }
    }
}
