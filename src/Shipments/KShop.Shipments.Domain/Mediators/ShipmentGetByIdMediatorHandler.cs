using FluentValidation;
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

    public class ShipmentGetByIdResponse
    {
        public ShipmentGetByIdResponse(Shipment shipment)
        {
            Shipment = shipment;
        }

        public Shipment Shipment { get; private set; }
    }
    public class ShipmentGetByIdRequest : IRequest<ShipmentGetByIdResponse>
    {
        public ShipmentGetByIdRequest(uint userID, Guid shipmentID)
        {
            UserID = userID;
            ShipmentID = shipmentID;
        }

        public uint UserID { get; private set; }
        public Guid ShipmentID { get; private set; }
    }
    public class ShipmentGetByIdHandler : IRequestHandler<ShipmentGetByIdRequest, ShipmentGetByIdResponse>
    {
        private readonly ILogger<ShipmentGetByIdHandler> _logger;
        private readonly ShipmentContext _shipmentContext;

        public ShipmentGetByIdHandler(ILogger<ShipmentGetByIdHandler> logger, ShipmentContext shipmentContext)
        {
            _logger = logger;
            _shipmentContext = shipmentContext;
        }

        public async Task<ShipmentGetByIdResponse> Handle(ShipmentGetByIdRequest request, CancellationToken cancellationToken)
        {
            var shipment = await _shipmentContext.Shipments.FirstOrDefaultAsync(e => e.ID == request.ShipmentID);

            return new ShipmentGetByIdResponse(shipment);
        }
    }
}
