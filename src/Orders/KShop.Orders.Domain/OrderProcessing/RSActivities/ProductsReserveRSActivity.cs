using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Products;
using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.OrderPlacing.Activities
{
    public class ProductsReserveRSActivityLog
    {
        public Guid OrderID { get; set; }
    }

    public class ProductsReserveRSActivityArgs
    {
        public Guid OrderID { get; set; }
        public Guid ReserveID { get; set; }
        public uint CustomerID { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
    }

    public class ProductsReserveRSActivity
        : IActivity<ProductsReserveRSActivityArgs, ProductsReserveRSActivityLog>
    {
        private readonly ILogger<ProductsReserveRSActivity> _logger;
        private readonly IPublishEndpoint _pub;

        private readonly IRequestClient<ProductsReserveSvcRequest> _clReserve;
        private readonly IRequestClient<ProductsReserveCancelSvcRequest> _clReserveCancel;

        public ProductsReserveRSActivity(
            ILogger<ProductsReserveRSActivity> logger,
            IPublishEndpoint pub,
            IRequestClient<ProductsReserveSvcRequest> clCreate,
            IRequestClient<ProductsReserveCancelSvcRequest> clReserveCancel)
        {
            _logger = logger;
            _pub = pub;
            _clReserve = clCreate;
            _clReserveCancel = clReserveCancel;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ProductsReserveRSActivityArgs> context)
        {
            var response = await _clReserve.GetResponse<ProductsReserveSvcResponse>(
               new ProductsReserveSvcRequest()
               {
                   OrderID = context.Arguments.OrderID,
                   OrderPositions = context.Arguments.OrderPositions,
                   CustomerID = context.Arguments.CustomerID
               });

            if (response.Message.IsSuccess)
            {
                return context.Completed(
                    new ProductsReserveRSActivityLog
                    {
                        OrderID = context.Arguments.OrderID
                    });
            }
            else
            {
                return context.Faulted();
            }
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ProductsReserveRSActivityLog> context)
        {
            var response = await _clReserveCancel.GetResponse<ProductsReserveCancelSvcResponse>(
                new ProductsReserveCancelSvcRequest(context.Log.OrderID));

            return context.Compensated();
        }

    }
}
