﻿using KShop.Orders.Domain.ValueObjects;
using System.Collections.Generic;

namespace KShop.Orders.WebApi.DTOs
{
    public class OrderCreateDto
    {
        public IEnumerable<ProductStack> Positions { get; set; }
    }
}
