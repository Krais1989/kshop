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
    public class OrderPaySuccessActivity : BaseSagaActivity<OrderSagaState, IOrderPaySuccessEvent>
    {
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _dbContext;

        public OrderPaySuccessActivity(IPublishEndpoint pubEndpoint, ILogger<OrderPaySuccessActivity> logger, OrderContext orderContext)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
            _dbContext = orderContext;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderPaySuccessEvent> context, Behavior<OrderSagaState, IOrderPaySuccessEvent> next)
        {
            await base.Execute(context, next);
            var order = await _dbContext.Orders.FindAsync(context.Data.OrderID);
            order.Status = Order.EStatus.Shipping;
            await _dbContext.SaveChangesAsync();
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
