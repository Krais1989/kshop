using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Products;
using KShop.Communications.Contracts.ValueObjects;
using KShop.Orders.Domain.RoutingSlips;
using KShop.Orders.Domain.RoutingSlips.OrderInitialization;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Sagas
{

    public class OrderSagaState : SagaStateMachineInstance//, ISagaVersion
    {
        public Guid CorrelationId { get; set; }


        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public int CurrentState { get; set; }
        //public int Version { get; set; }
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
            //sagaConfigurator.UseMessageRetry(e =>
            //{
            //    e.Intervals(500, 5000, 10000, 1000, 1000);
            //});
            sagaConfigurator.UseInMemoryOutbox();
        }
    }

    public class OrderSagaStateMachine
        : MassTransitStateMachine<OrderSagaState>
    {
        private readonly ILogger<OrderSagaStateMachine> _logger;
        private readonly IEndpointNameFormatter _nameFormatter;

        public OrderSagaStateMachine(
            ILogger<OrderSagaStateMachine> logger,
            IEndpointNameFormatter nameFormatter)
        {
            _logger = logger;
            _nameFormatter = nameFormatter;

            //InstanceState(x => x.CurrentState, OrderCreation, ProductsReservation, OrderShipping);
            InstanceState(x => x.CurrentState);


            // ! NULL EXCEPTION !
            Event(() => OrderCreate_SagaRequest, x =>
            {
                x.CorrelateById(ctx => ctx.Message.OrderID);
                x.InsertOnInitial = true;
                x.SelectId(x => x.Message.OrderID);
                //x.SetSagaFactory(context => new OrderSagaState() { CorrelationId = context.CorrelationId ?? Guid.NewGuid() });
                x.SetSagaFactory(context => new OrderSagaState() { CorrelationId = context.Message.OrderID });
            });

            Event(() => OrderGetStatus_SagaRequest, x =>
            {
                x.CorrelateById(ctx => ctx.Message.OrderID);
                x.OnMissingInstance(cfg =>
                    cfg.ExecuteAsync(async ctx =>
                        await ctx.RespondAsync(new OrderGetStatusSagaResponse(-1, "Missing Saga Instance"))));
            });

            Event(() => RoutingSlip_Compelted, x =>
            {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
            });

            Event(() => RoutingSlip_Faulted, x =>
            {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
            });

            DuringAny(
                When(OrderGetStatus_SagaRequest)
                    .RespondAsync(async x => await x.Init<OrderGetStatusSagaResponse>(
                        new OrderGetStatusSagaResponse(x.Instance.CurrentState))));

            Initially(
                When(OrderCreate_SagaRequest)
                .ThenAsync(OnSagaCreateAsync)
                .TransitionTo(OrderCreation));

            During(OrderCreation,
                When(RoutingSlip_Compelted)
                .ThenAsync(OnRoutingSlipCompletedAsync)
                .TransitionTo(OrderShipping));
        }

        public State OrderCreation { get; private set; }
        public State ProductsReservation { get; private set; }
        public State OrderShipping { get; private set; }

        public Event<RoutingSlipCompleted> RoutingSlip_Compelted { get; private set; }
        public Event<RoutingSlipFaulted> RoutingSlip_Faulted { get; private set; }

        public Event<OrderPlacingSagaRequest> OrderCreate_SagaRequest { get; private set; }

        public Event<OrderGetStatusSagaRequest> OrderGetStatus_SagaRequest { get; private set; }


        private async Task OnSagaCreateAsync(BehaviorContext<OrderSagaState, OrderPlacingSagaRequest> bc)
        {
            bc.Instance.OrderID = bc.Data.OrderID;

            /* отправка события в консумер создания RS */
            /* 1. Новые события */

            //await bc.Publish(new OrderPlacingEvent());

            
            

            var q1Name = _nameFormatter.ExecuteActivity<OrderCreateRSActivity, OrderCreateRSActivityArgs>();
            var builder = new RoutingSlipBuilder(Guid.NewGuid());
            builder.AddActivity("CreateOrder", new Uri($"query:{q1Name}"));
            builder.AddVariable("CustomerID", bc.Data.CustomerID);
            builder.AddVariable("Positions", bc.Data.Positions);

            var rs = builder.Build();
            //await bc.CreateConsumeContext().Execute(rs);

            await bc.RespondAsync(new OrderPlacingSagaResponse() { IsSuccess = true, OrderID = bc.Instance.OrderID });
        }

        private async Task OnRoutingSlipCompletedAsync(BehaviorContext<OrderSagaState, RoutingSlipCompleted> bc)
        {
            _logger.LogInformation($"Routing Slip Completed");
        }
    }
}
