using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{

    public class OrderGetAllFluentValidatorDto
    {
    }

    public class OrderGetAllFluentValidator : AbstractValidator<OrderGetAllFluentValidatorDto>
    {
        public OrderGetAllFluentValidator()
        {
        }
    }

}
