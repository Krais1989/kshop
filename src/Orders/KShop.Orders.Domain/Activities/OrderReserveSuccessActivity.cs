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
    public class OrderReserveSuccessActivity : BaseSagaActivity<OrderSagaState, IOrderReserveSuccessEvent>
    {
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly IMediator _mediator;

        public OrderReserveSuccessActivity(IPublishEndpoint pubEndpoint, ILogger<OrderReserveSuccessActivity> logger, IMediator mediator)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
            _mediator = mediator;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderReserveSuccessEvent> context, Behavior<OrderSagaState, IOrderReserveSuccessEvent> next)
        {
            await base.Execute(context, next);

            var result = await _mediator.Send(new OrderSetStatusMediatorRequest()
            {
                OrderID = context.Data.OrderID,
                NewStatus = Order.EStatus.Processing
            });
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
