using FluentValidation;
using KShop.Shared.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.OrderProcessing.Validators
{

    public class OrderSubmitFluentValidatorDto
    {
        public List<ProductQuantity> OrderContent { get; set; }
    }

    public class OrderSubmitFluentValidator : AbstractValidator<OrderSubmitFluentValidatorDto>
    {
        public OrderSubmitFluentValidator()
        {
            /* Указано больше одной позиции */
            /*  */
        }
    }

}
