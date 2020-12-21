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
    public class OrderReserveFailureActivity : BaseSagaActivity<OrderSagaState, IOrderReserveFailureEvent>
    {
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _dbContext;

        public OrderReserveFailureActivity(IPublishEndpoint pubEndpoint, OrderContext dbContext, ILogger<OrderReserveFailureActivity> logger)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
            _dbContext = dbContext;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderReserveFailureEvent> context, Behavior<OrderSagaState, IOrderReserveFailureEvent> next)
        {
            await base.Execute(context, next);

            var order = await _dbContext.Orders.FindAsync(context.Data.OrderID);
            order.Status = Order.EStatus.Failed;
            await _dbContext.SaveChangesAsync();
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
