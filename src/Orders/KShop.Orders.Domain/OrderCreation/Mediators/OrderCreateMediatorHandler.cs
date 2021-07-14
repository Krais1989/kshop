using FluentValidation;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
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

    public class OrderCreateMediatorResponse : BaseResponse
    {
        public OrderCreateMediatorResponse(Guid orderID) : base()
        {
            OrderID = orderID;
        }

        public OrderCreateMediatorResponse(string error) : base(error)
        {
        }

        public Guid OrderID { get; set; }
    }
    public class OrderCreateMediatorRequest : IRequest<OrderCreateMediatorResponse>
    {
        public OrderCreateMediatorRequest(
            Guid orderID,
            uint customerID,
            Money orderPrice,
            List<ProductQuantity> orderContent)
        {
            OrderID = orderID;
            UserID = customerID;
            OrderPrice = orderPrice;
            OrderContent = orderContent;
        }

        public Guid OrderID { get; set; }
        public uint UserID { get; set; }
        public Money OrderPrice { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
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
            var validator_result = _validator.Validate(validatorDto);
            if (!validator_result.IsValid)
                return new OrderCreateMediatorResponse(validator_result.Errors.ToString());

            var entity = new Order
            {
                ID = request.OrderID,
                CustomerID = request.UserID,
                CreateDate = DateTime.UtcNow,
                Positions = request.OrderContent?.Select(e => new OrderPosition() { ProductID = e.ProductID, Quantity = e.Quantity }).ToList(),
                Logs = new List<OrderLog>(),
                Price = request.OrderPrice
            };
            entity.SetStatus(EOrderStatus.Created);

            _context.Add(entity);

            try
            {
                await _context.SaveChangesAsync();
                return new OrderCreateMediatorResponse(entity.ID);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cover strategy exception");
                return new OrderCreateMediatorResponse(e.Message);
            }
        }
    }
}
