using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Handlers;
using KShop.Orders.Domain.Sagas;
using KShop.ServiceBus;
using MassTransit;
using MassTransit.Courier.Contracts;
using MediatR;
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
        private readonly IMediator _mediator;

        public OrderCreateActivity(ILogger<OrderCreateActivity> logger, IMediator mediator)
            : base(logger)
        {
            _mediator = mediator;
        }

        public override async Task Execute(BehaviorContext<OrderSagaState, IOrderCreateSagaRequest> context, Behavior<OrderSagaState, IOrderCreateSagaRequest> next)
        {
            await base.Execute(context, next);
            var msg = context.Data;
            var response = await _mediator.Send(new OrderCreateMediatorRequest()
            {
                CustomerID = msg.CustomerID,
                Positions = msg.Positions
            });
            context.Instance.CorrelationId = response.OrderID;
            context.Instance.OrderPositions = context.Data.Positions;

            await next.Execute(context).ConfigureAwait(false);
        }
    }
}
