using KShop.Communications.Contracts.Orders;
using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.RoutingSlips.OrderInitialization
{
    public class OrderCreateRSActivityLog
    {
        public Guid? OrderID { get; set; }
    }

    public class OrderCreateRSActivityArgs
    {
        public Guid? OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
    }

    public class OrderCreateRSActivity
        : IActivity<OrderCreateRSActivityArgs, OrderCreateRSActivityLog>
    {
        private readonly ILogger<OrderCreateRSActivity> _logger;
        private readonly IPublishEndpoint _pub;

        private readonly IRequestClient<OrderCreateSvcRequest> _clCreate;
        private readonly IRequestClient<OrderSetStatusCancelledSvcRequest> _clCancel;

        public OrderCreateRSActivity(
            ILogger<OrderCreateRSActivity> logger,
            IPublishEndpoint pub,
            IRequestClient<OrderCreateSvcRequest> clCreate,
            IRequestClient<OrderSetStatusCancelledSvcRequest> clCancel)
        {
            _logger = logger;
            _pub = pub;
            _clCreate = clCreate;
            _clCancel = clCancel;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<OrderCreateRSActivityArgs> context)
        {
            var response = await _clCreate.GetResponse<OrderCreateSvcResponse>(
                new OrderCreateSvcRequest()
                {
                    OrderID = context.Arguments.OrderID,
                    CustomerID = context.Arguments.CustomerID,
                    Positions = context.Arguments.OrderPositions
                });

            if (response.Message.IsSuccess)
            {
                return context.Completed(
                    new OrderCreateRSActivityLog
                    {
                        OrderID = response.Message.OrderID
                    });
            }
            else
            {
                return context.Faulted();
            }
        }

        public async Task<CompensationResult> Compensate(CompensateContext<OrderCreateRSActivityLog> context)
        {
            var response = await _clCancel.GetResponse<OrderSetStatusSvcResponse>(new OrderSetStatusCancelledSvcRequest(context.Log.OrderID.Value));

            if (response.Message.IsSuccess)
            {

            }

            return context.Compensated();
        }

    }
}
