using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{

    public class PaymentCancelInternalFluentValidatorDto
    {
    }

    public class PaymentCancelInternalFluentValidator : AbstractValidator<PaymentCancelInternalFluentValidatorDto>
    {
        public PaymentCancelInternalFluentValidator()
        {
        }
    }

}
