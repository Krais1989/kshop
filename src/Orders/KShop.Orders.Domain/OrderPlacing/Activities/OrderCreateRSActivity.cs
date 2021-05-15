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
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
    }

    public class OrderCreateRSActivity
        : IActivity<OrderCreateRSActivityArgs, OrderCreateRSActivityLog>
    {
        private readonly ILogger _logger;
        private readonly IPublishEndpoint _pub;

        private readonly IRequestClient<OrderCreateSvcRequest> _clCreate;
        private readonly IRequestClient<OrderCancelSvcRequest> _clCancel;

        public OrderCreateRSActivity(
            ILogger logger,
            IPublishEndpoint pub,
            IRequestClient<OrderCreateSvcRequest> clCreate,
            IRequestClient<OrderCancelSvcRequest> clCancel)
        {
            _logger = logger;
            _pub = pub;
            _clCreate = clCreate;
            _clCancel = clCancel;
        }


        public async Task<ExecutionResult> Execute(ExecuteContext<OrderCreateRSActivityArgs> context)
        {
            var response = await _clCreate.GetResponse<OrderCreateSvcResponse>(new OrderCreateSvcRequest()
            {
                CorrelationID = context.CorrelationId.Value,
                CustomerID = context.Arguments.CustomerID
            });

            if (response.Message.IsSuccess)
            {
                return context.CompletedWithVariables(
                    new OrderCreateRSActivityLog
                    {
                        OrderID = response.Message.OrderID
                    },
                    new OrderCreateRSActivityArgs {
                        OrderID = response.Message.OrderID.Value
                    });
            }
            else
            {
                return context.Faulted();
            }
        }

        public async Task<CompensationResult> Compensate(CompensateContext<OrderCreateRSActivityLog> context)
        {
            var response = await _clCancel.GetResponse<OrderCancelSvcResponse>(new OrderCancelSvcRequest()
            {
                OrderID = context.Log.OrderID.Value
            });

            if (response.Message.IsSuccess)
            {

            }

            return context.Compensated();
        }

    }
}
