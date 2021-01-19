using FluentValidation;
using KShop.Products.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KShop.Products.Domain.Mediators
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
        private readonly ILogger<ProductsReserveMediatorHandler> _logger;
        private readonly ProductsContext _catalogueContext;

        public OrderReserveCompensationRequestHandler(ILogger<ProductsReserveMediatorHandler> logger, ProductsContext catalogueContext)
        {
            _logger = logger;
            _catalogueContext = catalogueContext;
        }

        public async Task<OrderReserveCompensationResponse> Handle(OrderReserveCompensationRequest request, CancellationToken cancellationToken)
        {
            /* Компенсация */

            //var qRems = _catalogueContext.ProductReserves.Where(e => e.OrderID == request.OrderID);
            //_catalogueContext.ProductReserves.RemoveRange()
            return new OrderReserveCompensationResponse();
        }
    }
}
