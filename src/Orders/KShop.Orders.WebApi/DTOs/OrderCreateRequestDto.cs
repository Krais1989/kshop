﻿using KShop.Communications.Contracts.ValueObjects;
using System.Collections.Generic;

namespace KShop.Orders.WebApi.DTOs
{
    public class OrderCreateRequestDto
    {
        public Dictionary<int,int> Positions { get; set; }
    }
}
