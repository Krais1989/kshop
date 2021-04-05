
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KShop.Orders.Domain.Handlers;
using MassTransit.Courier;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KShop.Orders.Domain.RoutingSlips
{
    public class OrderCreateCourierArguments
    {
        public int CustomerID{ get; set; }
        public IDictionary<int, int> Positions { get; set; }
    }

    public class OrderCreateCourierLog
    {
        public Guid OrderID { get; set; }
    }

    public class OrderCreateCourierActivity : IActivity<OrderCreateCourierArguments, OrderCreateCourierLog>
    {
        private readonly ILogger<OrderCreateCourierActivity> _logger;
        private readonly IMediator _mediator;

        public OrderCreateCourierActivity(ILogger<OrderCreateCourierActivity> logger, IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<OrderCreateCourierArguments> context)
        {
            var orderId = Guid.NewGuid();
            _logger.LogInformation($"Order created: {orderId}");

            var request = new OrderCreateMediatorRequest() {
                CustomerID = context.Arguments.CustomerID,
                Positions = context.Arguments.Positions
            };
            var response = await _mediator.Send(request);

            return context.CompletedWithVariables(new { OrderID = response.OrderID }, new { OrderID = response.OrderID });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<OrderCreateCourierLog> context)
        {
            _logger.LogInformation($"COMPENSATE> OrderID: {context.Log.OrderID}");
            var request = new OrderSetStatusMediatorRequest(context.Log.OrderID, Persistence.Entities.Order.EStatus.Failed);
            var response = await _mediator.Send(request);            
            return context.Compensated();
        }
    }


}
