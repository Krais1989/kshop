using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Sagas;
using KShop.ServiceBus;
using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Activities
{
    public class OrderCreateActivity : BaseSagaActivity<OrderSagaState, IOrderCreateSagaRequest>
    {
        private readonly IPublishEndpoint _pubEndpoint;

        public OrderCreateActivity(IPublishEndpoint pubEndpoint, ILogger<OrderCreateActivity> logger)
            : base(logger)
        {
            _pubEndpoint = pubEndpoint;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderCreateSagaRequest> context, Behavior<OrderSagaState, IOrderCreateSagaRequest> next)
        {
            await base.Execute(context, next);
            context.Instance.CorrelationId = context.Data.OrderID;
            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
