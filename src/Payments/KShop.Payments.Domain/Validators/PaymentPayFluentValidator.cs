using FluentValidation;
using KShop.Communications.Contracts.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Payments.Domain.Validators
{
    public class PaymentPayValidatorDto
    {
        public PaymentPayValidatorDto(string externalPaymentID, EPaymentPlatformType platformType)
        {
            ExternalPaymentID = externalPaymentID;
            PlatformType = platformType;
        }

        public string ExternalPaymentID { get; set; }
        public EPaymentPlatformType PlatformType { get; set; }
    }

    public class PaymentPayFluentValidator : AbstractValidator<PaymentPayValidatorDto>
    {
        public PaymentPayFluentValidator()
        {
            RuleFor(e => e.PlatformType).NotEqual(EPaymentPlatformType.None);
            RuleFor(e => e.ExternalPaymentID).NotNull().NotEmpty();
        }
    }

}
