using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{

    public class PaymentCreateValidatorDto
    {
    }

    public class PaymentCreateFluentValidator : AbstractValidator<PaymentCreateValidatorDto>
    {
        public PaymentCreateFluentValidator()
        {
        }
    }

}
