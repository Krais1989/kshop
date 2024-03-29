﻿using FluentValidation;
using KShop.Orders.Domain.OrderProcessing.Validators;
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

    public class OrderSubmitMediatorResponse : BaseResponse
    {
        public OrderSubmitMediatorResponse(Guid orderID) : base()
        {
            OrderID = orderID;
        }

        public OrderSubmitMediatorResponse(string error) : base(error)
        {
        }

        public Guid OrderID { get; private set; }
    }
    public class OrderSubmitMediatorRequest : IRequest<OrderSubmitMediatorResponse>
    {
        public OrderSubmitMediatorRequest(
            Address address,
            EPaymentProvider paymentProvider,
            EShippingMethod shippingMethod,
            List<ProductQuantity> orderContent,
            uint userID)
        {
            Address = address;
            PaymentProvider = paymentProvider;
            ShippingMethod = shippingMethod;
            OrderContent = orderContent;
            UserID = userID;
        }

        public Address Address { get; private set; }
        public EPaymentProvider PaymentProvider { get; private set; }
        public EShippingMethod ShippingMethod { get; private set; }
        public List<ProductQuantity> OrderContent { get; private set; }
        public uint UserID { get; private set; }
    }
    public class OrderSubmitMediatorHandler : IRequestHandler<OrderSubmitMediatorRequest, OrderSubmitMediatorResponse>
    {
        private readonly ILogger<OrderSubmitMediatorHandler> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly IValidator<OrderSubmitFluentValidatorDto> _validator;

        public OrderSubmitMediatorHandler(ILogger<OrderSubmitMediatorHandler> logger, IPublishEndpoint pubEndpoint, IValidator<OrderSubmitFluentValidatorDto> validator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _validator = validator;
        }


        public async Task<OrderSubmitMediatorResponse> Handle(OrderSubmitMediatorRequest request, CancellationToken cancellationToken)
        {
            var valid_dto = new OrderSubmitFluentValidatorDto() { };
            var valid_result = _validator.Validate(valid_dto);

            if (!valid_result.IsValid)
                return new OrderSubmitMediatorResponse(valid_result.Errors.ToString());

            var orderCreateRequest = new OrderSubmitSagaRequest
            (
                orderID: Guid.NewGuid(),
                customerID: request.UserID,
                paymentProvider: request.PaymentProvider,
                shippingMethod: request.ShippingMethod,
                orderContent: request.OrderContent,
                address: request.Address
            );

            await _pubEndpoint.Publish(orderCreateRequest);
            return new OrderSubmitMediatorResponse(orderCreateRequest.OrderID);
        }
    }
}
