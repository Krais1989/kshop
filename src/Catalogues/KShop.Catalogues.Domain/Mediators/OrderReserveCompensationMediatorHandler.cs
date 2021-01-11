using FluentValidation;
using KShop.Catalogues.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Catalogues.Domain.Mediators
{

    public class OrderReserveCompensationResponse
    {
    }
    public class OrderReserveCompensationRequest : IRequest<OrderReserveCompensationResponse>
    {
        public Guid OrderID { get; set; }
    }
    public class OrderReserveCompensationRequestHandler : IRequestHandler<OrderReserveCompensationRequest, OrderReserveCompensationResponse>
    {
        private readonly ILogger<OrderReserveMediatorHandler> _logger;
        private readonly CatalogueContext _catalogueContext;

        public async Task<OrderReserveCompensationResponse> Handle(OrderReserveCompensationRequest request, CancellationToken cancellationToken)
        {
            _catalogueContext.ProductReserves.Remo
            return new OrderReserveCompensationResponse();
        }
    }
}
