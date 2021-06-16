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

    public class OrderGetAllResponse : BaseResponse
    {
        public List<Order> Orders { get; set; }
    }
    public class OrderGetAllRequest : IRequest<OrderGetAllResponse>
    {
        public ulong CustomerID { get; set; }
    }
    public class OrderGetAllHandler : IRequestHandler<OrderGetAllRequest, OrderGetAllResponse>
    {
        private readonly ILogger<OrderGetAllHandler> _logger;
        private readonly IValidator<OrderGetAllFluentValidatorDto> _validator;
        private readonly OrderContext _orderContext;

        public OrderGetAllHandler(
            ILogger<OrderGetAllHandler> logger,
            IValidator<OrderGetAllFluentValidatorDto> validator,
            OrderContext orderContext)
        {
            _logger = logger;
            _validator = validator;
            _orderContext = orderContext;
        }

        public async Task<OrderGetAllResponse> Handle(OrderGetAllRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new OrderGetAllFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var result = await _orderContext.Orders.AsNoTracking().Where(e => e.CustomerID == request.CustomerID).ToListAsync();

            return new OrderGetAllResponse()
            {
                Orders = result
            };
        }
    }
}
