using FluentValidation;
using KShop.Orders.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain.Validators
{

    public class OrderCreateRevertFluentValidatorDto
    {
        public Order.EStatus Status { get; set; }
    }

    public class OrderCreateRevertFluentValidator : AbstractValidator<OrderCreateRevertFluentValidatorDto>
    {
        public OrderCreateRevertFluentValidator()
        {
            // 
        }
    }

}
