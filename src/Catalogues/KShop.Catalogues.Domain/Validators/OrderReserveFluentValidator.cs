using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Catalogues.Domain.Validators
{
    public class OrderReserveFluentValidatorDto
    {
    }

    public class OrderReserveFluentValidator : AbstractValidator<OrderReserveFluentValidatorDto>
    {
        public OrderReserveFluentValidator()
        {
        }
    }
}
