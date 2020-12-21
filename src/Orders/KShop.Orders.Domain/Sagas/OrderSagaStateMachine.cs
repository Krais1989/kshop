using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.Activities;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.Extensions.Logging;
using System;

namespace KShop.Orders.Domain.Sagas
{
    public class OrderSagaState : SagaStateMachineInstance, ISagaVersion
    {
        public int CurrentState { get; set; }

        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
    }

    public class OrderSagaStateMachineDefinition
        : SagaDefinition<OrderSagaState>
    {
        public OrderSagaStateMachineDefinition()
        {
            //ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderSagaState> sagaConfigurator)
        {
            sagaConfigurator.UseMessageRetry(e => e.Intervals(500, 5000, 10000));
            sagaConfigurator.UseInMemoryOutbox();
        }
    }

    public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaState>
    {
        private readonly ILogger<OrderSagaStateMachine> _logger;

        public State Reserving { get; private set; }
        public State Processing { get; private set; }
        public Event<IOrderCreateSagaRequest> OrderCreateRequest { get; private set; }
        public Event<IOrderReserveSuccessEvent> OrderReserveSuccess { get; private set; }
        public Event<IOrderReserveFailureEvent> OrderReserveFailure { get; private set; }
        public Event<IOrderPaySuccessEvent> OrderPaySuccess { get; private set; }
        public Event<IOrderPayFailureEvent> OrderPayFailure { get; private set; }


        public Event<Fault<IOrderReserveEvent>> FaultTest { get; private set; }

        public Event<CheckOrderSagaRequest> OrderCheckRequested { get; private set; }

        public OrderSagaStateMachine(ILogger<OrderSagaStateMachine> logger)
        //public OrderSagaStateMachine(ILogger<OrderSagaStateMachine> logger, IBus bus)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState, Reserving, Processing);
            Event(() => OrderCreateRequest, x => x.CorrelateById(ctx => ctx.Message.OrderID));
            Event(() => OrderReserveSuccess, x => x.CorrelateById(ctx => ctx.Message.OrderID));
            Event(() => OrderReserveFailure, x => x.CorrelateById(ctx => ctx.Message.OrderID));
            Event(() => OrderPaySuccess, x => x.CorrelateById(ctx => ctx.Message.OrderID));
            Event(() => OrderPayFailure, x => x.CorrelateById(ctx => ctx.Message.OrderID));

            //Event(() => FaultTest, x => x.CorrelateById(m => m.));
            Event(() => FaultTest, x => x.CorrelateById(m => m.Message.Message.OrderID));

            Event(() => OrderCheckRequested, x =>
            {
                x.CorrelateById(ctx => ctx.Message.OrderID);
                x.OnMissingInstance(cfg =>
                    cfg.ExecuteAsync(async ctx =>
                        await ctx.RespondAsync<IOrderNotFoundResponse>(
                            new {
                                OrderID = ctx.Message.OrderID
                            }))
                    );
            });

            DuringAny(
                When(OrderCheckRequested)
                    .RespondAsync(async x => await x.Init<ICheckOrderSagaResponse>(new CheckOrderSagaResponse()
                    {
                        OrderID = x.Instance.CorrelationId,
                        State = x.Instance.CurrentState
                    })));

            Initially(
                When(OrderCreateRequest)
                    .Activity(x => x.OfType<OrderCreateActivity>())
                    .PublishAsync(async x => await x.Init<IOrderReserveEvent>(new
                    {
                        OrderID = x.Data.OrderID,
                        Positions = x.Data.Positions
                    }))
                    .RespondAsync(async x => await x.Init<IOrderCreateSagaResponse>(new OrderCreateSagaResponse() { OrderID = x.Data.OrderID }))
                    .TransitionTo(Reserving));

            During(Reserving,
                When(OrderReserveSuccess)
                    .Activity(x => x.OfType<OrderReserveSuccessActivity>())
                    .PublishAsync(async x => await x.Init<IOrderPayEvent>(new { OrderID = x.Data.OrderID, Price = 10 }))
                    .TransitionTo(Processing),
                When(OrderReserveFailure)
                    .Activity(x => x.OfType<OrderReserveFailureActivity>())
                    .Finalize());

            During(Processing,
                When(OrderPaySuccess)
                    .Activity(x => x.OfType<OrderPaySuccessActivity>())
                    .Finalize(),
                When(OrderPayFailure)
                    .Activity(x => x.OfType<OrderPayFailureActivity>())
                    .PublishAsync(async x => await x.Init<IOrderReserveCompensationEvent>(new { OrderID = x.Data.OrderID }))
                    .Finalize());

        }
    }
}
