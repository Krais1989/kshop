using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain.OrderStatusChanging.Validators
{

    public class OrderSetStatusFluentValidatorDto
    {
    }

    public class OrderSetStatusFluentValidator : AbstractValidator<OrderSetStatusFluentValidatorDto>
    {
        public OrderSetStatusFluentValidator()
        {
        }
    }

}
