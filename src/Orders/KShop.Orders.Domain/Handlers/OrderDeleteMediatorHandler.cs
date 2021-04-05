using FluentValidation;
using KShop.Orders.Domain.Validators;
using KShop.Orders.Persistence;
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
        private readonly OrderContext _orderContext;

        public OrderDeleteMediatorRequestHandler(ILogger<OrderDeleteMediatorRequestHandler> logger, IValidator<OrderDeleteFluentValidatorDto> validator, OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderDeleteMediatorResponse> Handle(OrderDeleteMediatorRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderContext.Orders.FindAsync(request.OrderID);

            var validatorDto = new OrderDeleteFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            _orderContext.Remove(order);
            await _orderContext.SaveChangesAsync();
            return new OrderDeleteMediatorResponse();
        }
    }
}
