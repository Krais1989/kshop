using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain
{

    public class PaymentCreatedValidatorDto
    {
    }

    public class PaymentCreatedFluentValidator : AbstractValidator<PaymentCreatedValidatorDto>
    {
        public PaymentCreatedFluentValidator()
        {
        }
    }

}
