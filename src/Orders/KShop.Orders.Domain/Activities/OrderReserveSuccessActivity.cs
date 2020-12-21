using Automatonymous;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Sagas;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using KShop.ServiceBus;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Activities
{
    public class OrderReserveSuccessActivity : BaseSagaActivity<OrderSagaState, IOrderReserveSuccessEvent>
    {
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _dbContext;

        public OrderReserveSuccessActivity(IPublishEndpoint pubEndpoint, ILogger<OrderReserveSuccessActivity> logger, OrderContext orderContext)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
            _dbContext = orderContext;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderReserveSuccessEvent> context, Behavior<OrderSagaState, IOrderReserveSuccessEvent> next)
        {
            await base.Execute(context, next);

            var order = await _dbContext.Orders.FindAsync(context.Data.OrderID);
            order.Status = Order.EStatus.Processing;
            await _dbContext.SaveChangesAsync();            
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
