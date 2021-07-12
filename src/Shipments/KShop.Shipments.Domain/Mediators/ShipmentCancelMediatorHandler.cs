using FluentValidation;
using KShop.Shared.Domain.Contracts;
using KShop.Shipments.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain
{

    public class ShipmentCancelMediatorResponse : BaseResponse
    {
    }
    public class ShipmentCancelMediatorRequest : IRequest<ShipmentCancelMediatorResponse>
    {
        public ShipmentCancelMediatorRequest(uint userID, Guid shipmentID)
        {
            UserID = userID;
            ShipmentID = shipmentID;
        }

        public uint UserID { get; private set; }
        public Guid ShipmentID { get; private set; }
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
