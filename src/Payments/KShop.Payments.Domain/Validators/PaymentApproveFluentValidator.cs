using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{


    public class PaymentApproveValidatorDto
    {
    }

    public class PaymentApproveFluentValidator : AbstractValidator<PaymentApproveValidatorDto>
    {
        public PaymentApproveFluentValidator()
        {
        }
    }




}
