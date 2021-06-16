using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{

    public class OrderGetDetailsFluentValidatorDto
    {
    }

    public class OrderGetDetailsFluentValidator : AbstractValidator<OrderGetDetailsFluentValidatorDto>
    {
        public OrderGetDetailsFluentValidator()
        {
        }
    }

}
