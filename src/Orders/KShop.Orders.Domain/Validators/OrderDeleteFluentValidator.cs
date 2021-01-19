using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain.Validators
{

    public class OrderDeleteFluentValidatorDto
    {
    }

    public class OrderDeleteFluentValidator : AbstractValidator<OrderDeleteFluentValidatorDto>
    {
        public OrderDeleteFluentValidator()
        {
        }
    }

}
