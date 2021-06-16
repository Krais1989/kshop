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
    public class OrderSetStatusResponse : BaseResponse
    {
    }
    public class OrderSetStatusRequest : IRequest<OrderSetStatusResponse>
    {
        public Guid OrderID { get; set; }
        public EOrderStatus OrderStatus { get; set; }
    }
    public class OrderSetStatusMediatorHandler : IRequestHandler<OrderSetStatusRequest, OrderSetStatusResponse>
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

        public async Task<OrderSetStatusResponse> Handle(OrderSetStatusRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new OrderSetStatusFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var order = await _orderContext.Orders.SingleAsync(e => e.ID == request.OrderID, cancellationToken);
            order.Status = request.OrderStatus;
            await _orderContext.SaveChangesAsync(cancellationToken);

            return new OrderSetStatusResponse();
        }
    }
}
