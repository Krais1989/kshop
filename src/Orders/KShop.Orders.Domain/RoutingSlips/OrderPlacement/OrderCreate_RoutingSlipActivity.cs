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
    public class OrderInitialization_RoutingSlipActivity_Log
    {
        public Guid? OrderID { get; set; }
    }

    public class OrderCreate_RoutingSlipActivity_Args
    {
        public int CustomerID { get; set; }
    }

    public class OrderCreate_RoutingSlipActivity
        : IActivity<OrderCreate_RoutingSlipActivity_Args, OrderInitialization_RoutingSlipActivity_Log>
    {
        private readonly ILogger _logger;
        private readonly IPublishEndpoint _pub;

        private readonly IRequestClient<OrderCreate_BusRequest> _clExecute;
        private readonly IRequestClient<OrderCreateCompensate_BusRequest> _clCompensate;

        public OrderCreate_RoutingSlipActivity(ILogger logger, IPublishEndpoint pub, IRequestClient<OrderCreate_BusRequest> clExecute, IRequestClient<OrderCreateCompensate_BusRequest> clCompensate)
        {
            _logger = logger;
            _pub = pub;
            _clExecute = clExecute;
            _clCompensate = clCompensate;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<OrderInitialization_RoutingSlipActivity_Log> context)
        {
            var response = await _clCompensate.GetResponse<OrderCreateCompensate_BusResponse>(new OrderCreateCompensate_BusRequest()
            {
                CorrelationID = context.CorrelationId.Value,
                OrderID = context.Log.OrderID.Value
            });

            if (response.Message.IsSuccess)
            {

            }

            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<OrderCreate_RoutingSlipActivity_Args> context)
        {
            var response = await _clExecute.GetResponse<OrderCreate_BusResponse>(new OrderCreate_BusRequest() {
                CorrelationID = context.CorrelationId.Value,
                CustomerID = context.Arguments.CustomerID
            });

            if (response.Message.IsSuccess)
            {
                return context.Completed(new OrderInitialization_RoutingSlipActivity_Log
                { 
                    OrderID = response.Message.OrderID
                });
            }
            else
            {
                return context.Faulted();
            }
        }
    }
}
