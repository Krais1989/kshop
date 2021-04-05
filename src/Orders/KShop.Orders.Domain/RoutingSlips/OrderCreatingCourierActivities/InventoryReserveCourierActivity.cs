using MassTransit.Courier;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.RoutingSlips
{

    public class InventoryReserveCourierArguments
    {
        public int CustomerID { get; set; }
        public Guid OrderID { get; set; }
        public IDictionary<int,int> Positions { get; set; }
    }

    public class InventoryReserveCourierLog
    {
        public Guid ReserveID { get; set; }
    }

    public class InventoryReserveCourierActivity : IActivity<InventoryReserveCourierArguments, InventoryReserveCourierLog>
    {
        private readonly ILogger<InventoryReserveCourierActivity> _logger;
        private readonly IMediator _mediator;

        public InventoryReserveCourierActivity(ILogger<InventoryReserveCourierActivity> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<InventoryReserveCourierArguments> context)
        {
            var reserveId = Guid.NewGuid();
            _logger.LogInformation($"EXECUTE> OrderID: {context.Arguments.OrderID} ReserveID:{reserveId}");
            return context.Completed(new { ReserveID = reserveId });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<InventoryReserveCourierLog> context)
        {
            _logger.LogInformation($"COMPENSATE> ReserveID: {context.Log.ReserveID}");
            return context.Compensated();
        }
    }

}
