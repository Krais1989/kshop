using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Products.Domain.Validators
{
    public class ProductsReserveFluentValidatorDto
    {
    }

    public class ProductsReserveFluentValidator : AbstractValidator<ProductsReserveFluentValidatorDto>
    {
        public ProductsReserveFluentValidator()
        {
        }
    }
}
