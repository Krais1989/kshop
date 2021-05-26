using FluentValidation;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.ValueObjects;
using KShop.Orders.Domain.Validators;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Handlers
{

    public class OrderCreateMediatorResponse
    {
        public Guid OrderID { get; set; }
    }
    public class OrderCreateMediatorRequest : IRequest<OrderCreateMediatorResponse>
    {
        public Guid? OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap Positions { get; set; }

    }
    public class OrderCreateMediatorHandler : IRequestHandler<OrderCreateMediatorRequest, OrderCreateMediatorResponse>
    {
        private readonly ILogger<OrderCreateMediatorHandler> _logger;
        private readonly IValidator<OrderCreateFluentValidatorDto> _validator;
        private readonly OrderContext _context;

        public OrderCreateMediatorHandler(
            ILogger<OrderCreateMediatorHandler> logger,
            IValidator<OrderCreateFluentValidatorDto> validator,
            OrderContext context)
        {
            _logger = logger;
            _validator = validator;
            _context = context;
        }

        public async Task<OrderCreateMediatorResponse> Handle(OrderCreateMediatorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"MediatoR CreateOrder");

            var validatorDto = new OrderCreateFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var entity = new Order
            {
                ID = request.OrderID ?? default,
                CustomerID = request.CustomerID,
                CreateDate = DateTime.UtcNow,
                Positions = request.Positions?.Select(e => new OrderPosition() { ProductID = e.Key, Quantity = e.Value }).ToList(),
                Logs = new List<OrderLog>()
            };
            entity.SetStatus(Order.EStatus.Initialized);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return new OrderCreateMediatorResponse() { OrderID = entity.ID };
        }
    }
}
