using FluentValidation;
using KShop.Orders.Domain.Validators;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Handlers
{

    public class OrderSetStatusMediatorResponse
    {
    }
    public class OrderSetStatusMediatorRequest : IRequest<OrderSetStatusMediatorResponse>
    {
        public OrderSetStatusMediatorRequest(Guid orderID, Order.EStatus newStatus)
        {
            OrderID = orderID;
            NewStatus = newStatus;
        }

        public Guid OrderID { get; set; }
        public Order.EStatus NewStatus { get; set; }
    }
    public class OrderSetStatusMediatorHandler : IRequestHandler<OrderSetStatusMediatorRequest, OrderSetStatusMediatorResponse>
    {
        private readonly ILogger<OrderSetStatusMediatorHandler> _logger;
        private readonly IValidator<OrderSetStatusFluentValidatorDto> _validator;
        private readonly OrderContext _orderContext;

        public OrderSetStatusMediatorHandler(
            ILogger<OrderSetStatusMediatorHandler> logger,
            IValidator<OrderSetStatusFluentValidatorDto> validator,
            OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderSetStatusMediatorResponse> Handle(OrderSetStatusMediatorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"MediatoR OrderSetStatus: {request.OrderID} set status \"{request.NewStatus}\"");

            var validatorDto = new OrderSetStatusFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var order = await _orderContext.Orders.FindAsync(request.OrderID);
            order.Status = request.NewStatus;
            order.StatusDate = DateTime.UtcNow;
            await _orderContext.SaveChangesAsync();

            return new OrderSetStatusMediatorResponse();
        }
    }
}
