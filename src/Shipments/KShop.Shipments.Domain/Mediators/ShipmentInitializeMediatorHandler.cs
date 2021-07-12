using FluentValidation;
using KShop.Shared.Domain.Contracts;
using KShop.Shipments.Persistence;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Shipments.Domain
{

    public class ShipmentInitializeMediatorResponse
    {
        public ShipmentInitializeMediatorResponse(Guid shipmentID)
        {
            ShipmentID = shipmentID;
        }

        public Guid ShipmentID { get; private set; }
    }
    public class ShipmentInitializeMediatorRequest : IRequest<ShipmentInitializeMediatorResponse>
    {
        public ShipmentInitializeMediatorRequest(uint userID, Guid orderID)
        {
            UserID = userID;
            OrderID = orderID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
    }
    public class ShipmentInitializeMediatorHandler : IRequestHandler<ShipmentInitializeMediatorRequest, ShipmentInitializeMediatorResponse>
    {
        private readonly ILogger<ShipmentInitializeMediatorHandler> _logger;
        private readonly IValidator<ShipmentCreateFluentValidatorDto> _validator;

        private readonly ShipmentContext _shipmentContext;

        public ShipmentInitializeMediatorHandler(
            ILogger<ShipmentInitializeMediatorHandler> logger,
            IValidator<ShipmentCreateFluentValidatorDto> validator,
            ShipmentContext shipmentContext)
        {
            _logger = logger;
            _validator = validator;
            _shipmentContext = shipmentContext;
        }

        public async Task<ShipmentInitializeMediatorResponse> Handle(ShipmentInitializeMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new ShipmentCreateFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var shipment = new Shipment
            {
                OrderID = request.OrderID,
                Status = EShipmentStatus.Initializing,
                CreateDate = DateTime.UtcNow,
            };

            await _shipmentContext.AddAsync(shipment);
            await _shipmentContext.SaveChangesAsync();

            return new ShipmentInitializeMediatorResponse(shipment.ID);
        }
    }
}
