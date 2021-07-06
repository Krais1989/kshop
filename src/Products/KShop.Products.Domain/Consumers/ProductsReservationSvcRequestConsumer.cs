using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Products.Domain
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
                    CustomerID = context.Message.CustomerID,
                    OrderID = context.Message.OrderID,
                    OrderContent = context.Message.OrderContent
                };

                var res = await _mediator.Send(productsReserve);

                //if (context.RequestId.HasValue && context.ResponseAddress != null)
                //    await context.RespondAsync(new ProductsReserveSvcResponse { ProductsReserves = res.ReservationData, OrderPrice = res.OrderPrice });
            }
            catch (Exception e)
            {
                //if (context.RequestId.HasValue && context.ResponseAddress != null)
                //    await context.RespondAsync(new ProductsReserveSvcResponse { ErrorMessage = e.Message });
            }
        }
    }
}
