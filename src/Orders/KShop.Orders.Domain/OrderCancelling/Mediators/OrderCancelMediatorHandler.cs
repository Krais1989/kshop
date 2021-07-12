using FluentValidation;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{

    public class OrderCancelMediatorResponse : BaseResponse
    {
    }
    public class OrderCancelMediatorRequest : IRequest<OrderCancelMediatorResponse>
    {
        public OrderCancelMediatorRequest(uint userID, Guid orderID)
        {
            UserID = userID;
            OrderID = orderID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
    }
    public class OrderCancelMediatorHandler : IRequestHandler<OrderCancelMediatorRequest, OrderCancelMediatorResponse>
    {
        private readonly ILogger<OrderCancelMediatorHandler> _logger;
        private readonly IValidator<OrderCancelFluentValidatorDto> _validator;
        private readonly OrderContext _orderContext;

        public OrderCancelMediatorHandler(
            ILogger<OrderCancelMediatorHandler> logger,
            IValidator<OrderCancelFluentValidatorDto> validator,
            OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderCancelMediatorResponse> Handle(OrderCancelMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new OrderCancelFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var order = await _orderContext.Orders.FirstOrDefaultAsync(e => e.ID == request.OrderID && e.CustomerID == request.UserID);
            order.SetStatus(EOrderStatus.Cancelled);

            await _orderContext.SaveChangesAsync();

            return new OrderCancelMediatorResponse();
        }
    }
}
