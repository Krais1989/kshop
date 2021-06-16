
using KShop.Orders.Persistence;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderSetStatusSvcRequestConsumer
        : IConsumer<OrderSetStatusReservedSvcRequest>,
        IConsumer<OrderSetStatusPayedSvcRequest>,
        IConsumer<OrderSetStatusShippedSvcRequest>,
        IConsumer<OrderSetStatusFaultedSvcRequest>,
        IConsumer<OrderSetStatusRefundedSvcRequest>,
        IConsumer<OrderSetStatusCancelledSvcRequest>
    {

        private readonly ILogger<OrderSetStatusSvcRequestConsumer> _logger;
        private readonly OrderContext _orderContext;

        public OrderSetStatusSvcRequestConsumer(
            ILogger<OrderSetStatusSvcRequestConsumer> logger,
            OrderContext orderContext)
        {
            _logger = logger;
            _orderContext = orderContext;
        }

        private async Task Handle(ConsumeContext context, Guid orderId, EOrderStatus status, string comment)
        {
            try
            {
                var order = await _orderContext.Orders.FirstOrDefaultAsync(e => e.ID == orderId);
                order.SetStatus(status, comment);
                await _orderContext.SaveChangesAsync();

                if (context.RequestId.HasValue && context.ResponseAddress != null)
                    await context.RespondAsync(new OrderSetStatusSvcResponse());
            }
            catch (Exception e)
            {
                if (context.RequestId.HasValue && context.ResponseAddress != null)
                    await context.RespondAsync(new OrderSetStatusSvcResponse { ErrorMessage = e.Message });
            }
        }

        public async Task Consume(ConsumeContext<OrderSetStatusReservedSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Reserved, context.Message.Comment);
        }

        public async Task Consume(ConsumeContext<OrderSetStatusPayedSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Payed, context.Message.Comment);
        }

        public async Task Consume(ConsumeContext<OrderSetStatusShippedSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Shipped, context.Message.Comment);
        }

        public async Task Consume(ConsumeContext<OrderSetStatusFaultedSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Faulted, context.Message.Comment);
        }

        public async Task Consume(ConsumeContext<OrderSetStatusRefundedSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Refunded, context.Message.Comment);
        }

        public async Task Consume(ConsumeContext<OrderSetStatusCancelledSvcRequest> context)
        {
            await Handle(context, context.Message.OrderID, EOrderStatus.Cancelled, context.Message.Comment);
        }
    }
}
