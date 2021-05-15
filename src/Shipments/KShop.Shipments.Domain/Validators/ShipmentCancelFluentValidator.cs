using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Domain.ShipmentProcessing.Validators
{

    public class ShipmentCancelFluentValidatorDto
    {
    }

    public class ShipmentCancelFluentValidator : AbstractValidator<ShipmentCancelFluentValidatorDto>
    {
        public ShipmentCancelFluentValidator()
        {
        }
    }

}
