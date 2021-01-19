using FluentValidation;
using KShop.Orders.Domain.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Handlers
{

    public class OrderDeleteMediatorResponse
    {
    }
    public class OrderDeleteMediatorRequest : IRequest<OrderDeleteMediatorResponse>
    {
        public OrderDeleteMediatorRequest(Guid orderID)
        {
            OrderID = orderID;
        }

        public Guid OrderID { get; private set; }
    }
    public class OrderDeleteMediatorRequestHandler : IRequestHandler<OrderDeleteMediatorRequest, OrderDeleteMediatorResponse>
    {
        private readonly ILogger<OrderDeleteMediatorRequestHandler> _logger;
        private readonly IValidator<OrderDeleteFluentValidatorDto> _validator;

        public async Task<OrderDeleteMediatorResponse> Handle(OrderDeleteMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new OrderDeleteFluentValidatorDto() { };
            _validator.Validate(validatorDto);
            return new OrderDeleteMediatorResponse();
        }
    }
}
