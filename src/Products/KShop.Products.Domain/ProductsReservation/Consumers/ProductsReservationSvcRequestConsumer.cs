using KShop.Communications.Contracts.Products;
using KShop.Products.Domain.ProductsReservation.Mediators;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Products.Domain.ProductsReservation.Consumers
{
    public class ProductsReservationSvcRequestConsumer : IConsumer<ProductsReserveSvcRequest>
    {
        private readonly ILogger<ProductsReservationSvcRequestConsumer> _logger;
        private readonly IMediator _mediator;

        public ProductsReservationSvcRequestConsumer(ILogger<ProductsReservationSvcRequestConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ProductsReserveSvcRequest> context)
        {
            try
            {
                var productsReserve = new ProductsReserveMediatorRequest()
                {
                    CustomerID = context.Message.CustomerID
                };

                var res = await _mediator.Send(productsReserve);

                await context.RespondAsync(new ProductsReserveMediatorResponse(res.ReservationData));
            }
            catch (Exception e)
            {
                await context.RespondAsync(new ProductsReserveMediatorResponse(e.Message));
            }
        }
    }
}
