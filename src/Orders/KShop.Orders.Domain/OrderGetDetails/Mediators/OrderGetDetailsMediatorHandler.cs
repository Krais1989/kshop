using FluentValidation;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderGetDetailsResponse : BaseResponse
    {
        public OrderGetDetailsResponse(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }
    public class OrderGetDetailsRequest : IRequest<OrderGetDetailsResponse>
    {
        public OrderGetDetailsRequest(uint userID, Guid orderID)
        {
            UserID = userID;
            OrderID = orderID;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
    }
    public class OrderGetDetailsHandler : IRequestHandler<OrderGetDetailsRequest, OrderGetDetailsResponse>
    {
        private readonly ILogger<OrderGetDetailsHandler> _logger;
        private readonly IValidator<OrderGetDetailsFluentValidatorDto> _validator;
        private readonly OrderContext _orderContext;

        public OrderGetDetailsHandler(
            ILogger<OrderGetDetailsHandler> logger,
            IValidator<OrderGetDetailsFluentValidatorDto> validator,
            OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderGetDetailsResponse> Handle(OrderGetDetailsRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new OrderGetDetailsFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var result = await _orderContext.Orders.AsNoTracking().FirstOrDefaultAsync(e => e.ID == request.OrderID && e.CustomerID == request.UserID);

            return new OrderGetDetailsResponse(result);
        }
    }
}
