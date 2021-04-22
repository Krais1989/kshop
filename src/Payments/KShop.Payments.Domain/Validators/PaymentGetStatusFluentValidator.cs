using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{

    public class PaymentGetStatusFluentValidatorDto
    {
    }

    public class PaymentGetStatusFluentValidator : AbstractValidator<PaymentGetStatusFluentValidatorDto>
    {
        public PaymentGetStatusFluentValidator()
        {
        }
    }

}
