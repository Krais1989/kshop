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

    public class OrderPlacingMediatorResponse
    {
        public Guid OrderID { get; set; }
    }
    public class OrderPlacingMediatorRequest : IRequest<OrderPlacingMediatorResponse>
    {
        public string Address { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
        public uint UserID { get; set; }
    }
    public class OrderPlacingMediatorHandler : IRequestHandler<OrderPlacingMediatorRequest, OrderPlacingMediatorResponse>
    {
        private readonly ILogger<OrderPlacingMediatorHandler> _logger;
        private readonly IPublishEndpoint _pubEndpoint;

        public OrderPlacingMediatorHandler(ILogger<OrderPlacingMediatorHandler> logger, IPublishEndpoint pubEndpoint)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
        }

        //private readonly IValidator<OrderPlacingMediatorFluentValidatorDto> _validator;

        public async Task<OrderPlacingMediatorResponse> Handle(OrderPlacingMediatorRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new OrderPlacingMediatorFluentValidatorDto() { };
            //_validator.Validate(validatorDto);
            var orderCreateRequest = new OrderSubmitSagaRequest
            {
                OrderID = Guid.NewGuid(),
                Customer = request.UserID,
                PaymentProvider = request.PaymentProvider,
                ShippingMethod = request.ShippingMethod,
                OrderContent = request.OrderContent,
                Address = request.Address,
            };

            await _pubEndpoint.Publish(orderCreateRequest);
            return new OrderPlacingMediatorResponse() { OrderID = orderCreateRequest.OrderID };
        }
    }
}
