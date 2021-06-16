using FluentValidation;
using KShop.Orders.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain
{

    public class OrderCreateRevertFluentValidatorDto
    {
        public EOrderStatus Status { get; set; }
    }

    public class OrderCreateRevertFluentValidator : AbstractValidator<OrderCreateRevertFluentValidatorDto>
    {
        public OrderCreateRevertFluentValidator()
        {
            // 
        }
    }

}
