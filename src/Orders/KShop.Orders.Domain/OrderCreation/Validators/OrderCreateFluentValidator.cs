using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain
{

    public class OrderCreateFluentValidatorDto
    {
    }

    public class OrderCreateFluentValidator : AbstractValidator<OrderCreateFluentValidatorDto>
    {
        public OrderCreateFluentValidator()
        {
        }
    }

}
