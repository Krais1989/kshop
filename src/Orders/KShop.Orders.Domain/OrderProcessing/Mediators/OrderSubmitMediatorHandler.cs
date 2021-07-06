using FluentValidation;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{

    public class OrderSubmitMediatorResponse
    {
        public Guid OrderID { get; set; }
    }
    public class OrderSubmitMediatorRequest : IRequest<OrderSubmitMediatorResponse>
    {
        public Address Address { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
        public uint UserID { get; set; }
    }
    public class OrderSubmitMediatorHandler : IRequestHandler<OrderSubmitMediatorRequest, OrderSubmitMediatorResponse>
    {
        private readonly ILogger<OrderSubmitMediatorHandler> _logger;
        private readonly IPublishEndpoint _pubEndpoint;

        public OrderSubmitMediatorHandler(ILogger<OrderSubmitMediatorHandler> logger, IPublishEndpoint pubEndpoint)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
        }

        //private readonly IValidator<OrderPlacingMediatorFluentValidatorDto> _validator;

        public async Task<OrderSubmitMediatorResponse> Handle(OrderSubmitMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new OrderPlacingMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);
            var orderCreateRequest = new OrderSubmitSagaRequest
            {
                OrderID = Guid.NewGuid(),
                CustomerID = request.UserID,
                PaymentProvider = request.PaymentProvider,
                ShippingMethod = request.ShippingMethod,
                OrderContent = request.OrderContent,
                Address = request.Address,
            };

            await _pubEndpoint.Publish(orderCreateRequest);
            return new OrderSubmitMediatorResponse() { OrderID = orderCreateRequest.OrderID };
        }
    }
}
