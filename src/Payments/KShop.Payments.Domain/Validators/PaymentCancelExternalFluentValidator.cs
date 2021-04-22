using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{

    public class PaymentCancelExternalValidatorDto
    {
    }

    public class PaymentCancelExternalFluentValidator : AbstractValidator<PaymentCancelExternalValidatorDto>
    {
        public PaymentCancelExternalFluentValidator()
        {
        }
    }

}
