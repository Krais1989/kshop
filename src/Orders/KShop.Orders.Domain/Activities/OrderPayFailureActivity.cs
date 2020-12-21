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

    public class OrderPayFailureActivity : BaseSagaActivity<OrderSagaState, IOrderPayFailureEvent>
    {
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _dbContext;

        public OrderPayFailureActivity(IPublishEndpoint pubEndpoint, ILogger<OrderPayFailureActivity> logger, OrderContext orderContext)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
            _dbContext = orderContext;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderPayFailureEvent> context, Behavior<OrderSagaState, IOrderPayFailureEvent> next)
        {
            await base.Execute(context, next);
            var order = await _dbContext.Orders.FindAsync(context.Data.OrderID);
            order.Status = Order.EStatus.Failed;
            await _dbContext.SaveChangesAsync();
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
