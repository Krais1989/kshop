using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.ValueObjects;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KShop.Orders.Domain.Sagas
{
    public class OrderSagaState : SagaStateMachineInstance, ISagaVersion
    {
        public int CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
        public Dictionary<int, int> OrderPositions { get; set; } = new Dictionary<int, int>();
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
            //sagaConfigurator.UseCircuitBreaker(e => {
            //    e.ActiveThreshold = 10;
            //    e.TrackingPeriod = TimeSpan.FromSeconds(10);

            //    e.ResetInterval = TimeSpan.FromSeconds(10);
            //    e.TripThreshold = 10;
            //});
            sagaConfigurator.UseMessageRetry(e =>
            {
                e.Intervals(500, 5000, 10000);
            });
            sagaConfigurator.UseInMemoryOutbox();
        }
    }

    public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaState>
    {
        private readonly ILogger<OrderSagaStateMachine> _logger;

        /// <summary>
        /// Создание платежа
        /// </summary>
        public State Reserving { get; private set; }
        /// <summary>
        /// Оплата 
        /// </summary>
        public State Processing { get; private set; }
        /// <summary>
        /// Доставка
        /// </summary>
        public State Shipping { get; private set; } 

        public Event<OrderCreate_SagaRequest> OrderCreate_SagaRequest { get; private set; }
        public Event<OrderGetStatus_SagaRequest> OrderGetStatus_SagaRequest { get; private set; }

        public Event<>

        public OrderSagaStateMachine(ILogger<OrderSagaStateMachine> logger)
        //public OrderSagaStateMachine(ILogger<OrderSagaStateMachine> logger, IBus bus)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState, Reserving, Processing, Shipping);
            Event(() => OrderCreate_SagaRequest, x =>
            {
                x.InsertOnInitial = true;
                x.SelectId(x => Guid.NewGuid());
                x.SetSagaFactory(context => new OrderSagaState() { CorrelationId = context.CorrelationId ?? Guid.NewGuid() });
            });

            Event(() => OrderGetStatus_SagaRequest, x =>
            {
                x.CorrelateById(ctx => ctx.Message.OrderID);
                x.OnMissingInstance(cfg =>
                    cfg.ExecuteAsync(async ctx =>
                        await ctx.RespondAsync(new OrderGetStatus_SagaResponse(-1, "Missing Saga Instance"))));
            });

            DuringAny(
                When(OrderGetStatus_SagaRequest)
                    .RespondAsync(async x => await x.Init<OrderGetStatus_SagaResponse>(
                        new OrderGetStatus_SagaResponse(x.Instance.CurrentState))));


            //Initially(
            //    When(OrderCreateRequest)
            //        .Activity(x => x.OfType<OrderCreateActivity>())
            //        .PublishAsync(async x => await x.Init<IOrderReserveEvent>(new
            //        {
            //            OrderID = x.Instance.CorrelationId,
            //            Positions = x.Instance.OrderPositions
            //        }))
            //        .RespondAsync(async x => await x.Init<IOrderCreateSagaResponse>(
            //            new OrderCreateSagaResponse() { OrderID = x.Instance.CorrelationId, IsSuccess = true }))
            //        .TransitionTo(Reserving));

            //During(Reserving,
            //    When(OrderReserveSuccess)
            //        .Activity(x => x.OfType<OrderReserveSuccessActivity>())
            //        .PublishAsync(async x => await x.Init<IOrderPayEvent>(new { OrderID = x.Data.OrderID, Price = 10 }))
            //        .TransitionTo(Processing),
            //    When(OrderReserveFailure)
            //        .Activity(x => x.OfType<OrderReserveFailureActivity>())
            //        .Finalize());

            //During(Processing,
            //    When(OrderPaySuccess)
            //        .Activity(x => x.OfType<OrderPaySuccessActivity>())
            //        .Finalize(),
            //    When(OrderPayFailure)
            //        .Activity(x => x.OfType<OrderPayFailureActivity>())
            //        .PublishAsync(async x => await x.Init<IOrderReserveCompensationEvent>(new { OrderID = x.Data.OrderID }))
            //        .Finalize());

        }
    }
}
