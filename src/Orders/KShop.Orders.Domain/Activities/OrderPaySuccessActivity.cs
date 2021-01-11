using Automatonymous;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Handlers;
using KShop.Orders.Domain.Sagas;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using KShop.ServiceBus;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Activities
{
    public class OrderPaySuccessActivity : BaseSagaActivity<OrderSagaState, IOrderPaySuccessEvent>
    {
        private readonly IMediator _mediator;

        public OrderPaySuccessActivity(ILogger<OrderPaySuccessActivity> logger, IMediator mediator)
            : base(logger)
        {
            _mediator = mediator;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderPaySuccessEvent> context, Behavior<OrderSagaState, IOrderPaySuccessEvent> next)
        {
            await base.Execute(context, next);
            var result = await _mediator.Send(new OrderSetStatusMediatorRequest()
            {
                OrderID = context.Data.OrderID,
                NewStatus = Order.EStatus.Shipping
            });
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
