using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Domain.Validators
{

    public class ShipmentCreateFluentValidatorDto
    {
    }

    public class ShipmentCreateFluentValidator : AbstractValidator<ShipmentCreateFluentValidatorDto>
    {
        public ShipmentCreateFluentValidator()
        {
        }
    }

}
