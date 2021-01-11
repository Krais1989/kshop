using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{

    public class PaymentRejectValidatorDto
    {
    }

    public class PaymentRejectFluentValidator : AbstractValidator<PaymentRejectValidatorDto>
    {
        public PaymentRejectFluentValidator()
        {
        }
    }

}
