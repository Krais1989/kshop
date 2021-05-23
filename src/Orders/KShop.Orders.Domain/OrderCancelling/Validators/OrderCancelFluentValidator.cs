using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Orders.Domain.OrderCancelling.Validators
{

    public class OrderCancelFluentValidatorDto
    {
    }

    public class OrderCancelFluentValidator : AbstractValidator<OrderCancelFluentValidatorDto>
    {
        public OrderCancelFluentValidator()
        {
        }
    }

}
