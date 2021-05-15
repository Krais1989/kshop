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

    public class OrderCreateRevertMediatorResponse
    {
    }
    public class OrderCreateRevertMediatorRequest : IRequest<OrderCreateRevertMediatorResponse>
    {
        public int OrderID { get; set; }
    }
    public class OrderCreateRevertMediatorHandler : IRequestHandler<OrderCreateRevertMediatorRequest, OrderCreateRevertMediatorResponse>
    {
        private readonly ILogger<OrderCreateRevertMediatorHandler> _logger;
        private readonly IValidator<OrderCreateRevertFluentValidatorDto> _validator;
        private readonly OrderContext _orderContext;

        public OrderCreateRevertMediatorHandler(ILogger<OrderCreateRevertMediatorHandler> logger, IValidator<OrderCreateRevertFluentValidatorDto> validator, OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderCreateRevertMediatorResponse> Handle(OrderCreateRevertMediatorRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderContext.Orders.FindAsync(request.OrderID);
            
            var validatorDto = new OrderCreateRevertFluentValidatorDto() { 
                Status = order.Status
            };
            _validator.Validate(validatorDto);

            order.Status = Persistence.Entities.Order.EStatus.Failed;
            await _orderContext.SaveChangesAsync();

            return new OrderCreateRevertMediatorResponse();
        }
    }
}
