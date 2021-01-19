using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Products;
using KShop.Products.Domain.Mediators;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Products.Domain.Consumers
{
    public interface ICompensate<out T> { }

    public class ProductsReserveConsumer : 
        IConsumer<ProductsReserve_BusEvent>,
        IConsumer<ProductsReserveCompensation_BusEvent>
    {
        private readonly ILogger<ProductsReserveConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;

        public ProductsReserveConsumer(
            ILogger<ProductsReserveConsumer> logger,
            IPublishEndpoint publishEndpoint, IMediator mediator)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<ProductsReserve_BusEvent> context)
        {
            try
            {
                await _mediator.Send(new ProductsReserveMediatorRequest()
                {
                    OrderID = context.Message.OrderID,
                    Positions = context.Message.Positions
                });
                                
                await _publishEndpoint.Publish(new ProductsReserveSuccess_BusEvent()
                {
                    CorrelationID = context.Message.CorrelationID
                });
            }
            catch (Exception e)
            {
                await _publishEndpoint.Publish(new ProductsReserveFailure_BusEvent()
                {
                    CorrelationID = context.Message.CorrelationID
                });
                throw;
            }
        }

        public Task Consume(ConsumeContext<ProductsReserveCompensation_BusEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
