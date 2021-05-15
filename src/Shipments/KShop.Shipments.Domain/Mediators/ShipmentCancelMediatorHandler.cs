using FluentValidation;
using KShop.Shipments.Domain.ExternalServices;
using KShop.Shipments.Domain.ShipmentProcessing.Validators;
using KShop.Shipments.Persistence;
using KShop.Shipments.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain.ShipmentProcessing.Mediators
{

    public class ShipmentCancelMediatorResponse
    {
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
    }
    public class ShipmentCancelMediatorRequest : IRequest<ShipmentCancelMediatorResponse>
    {
        public Guid ShipmentID { get; set; }
    }
    public class ShipmentCancelMediatorHandler : IRequestHandler<ShipmentCancelMediatorRequest, ShipmentCancelMediatorResponse>
    {
        private readonly ILogger<ShipmentCancelMediatorHandler> _logger;
        private readonly IValidator<ShipmentCancelFluentValidatorDto> _validator;
        private readonly ShipmentContext _shipmentContext;

        public ShipmentCancelMediatorHandler(
            ILogger<ShipmentCancelMediatorHandler> logger,
            IValidator<ShipmentCancelFluentValidatorDto> validator,
            ShipmentContext shipmentContext)
        {
            _logger = logger;
            _validator = validator;
            _shipmentContext = shipmentContext;
        }

        public async Task<ShipmentCancelMediatorResponse> Handle(ShipmentCancelMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new ShipmentCancelFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var shipment = await _shipmentContext.Shipments.FirstOrDefaultAsync(e => e.ID == request.ShipmentID);
            shipment.Status = EShipmentStatus.Cancelling;
            shipment.CompleteDate = DateTime.UtcNow;
            await _shipmentContext.SaveChangesAsync();

            return new ShipmentCancelMediatorResponse();
        }
    }
}
