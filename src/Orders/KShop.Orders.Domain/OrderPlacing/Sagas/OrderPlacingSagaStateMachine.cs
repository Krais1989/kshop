using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.Products;
using KShop.Communications.Contracts.Shipments;
using KShop.Orders.Domain.RoutingSlips.OrderInitialization;
using KShop.Orders.Domain.Sagas;
using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain.Orchestrations
{
    public class OrderPlacingSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderID { get; set; }
        public Guid ReserveID { get; set; }
        public Guid OrderPlacingRSTrackingNumber { get; set; }

        public int CustomerID { get; set; }
        public int CurrentState { get; set; }
        public IDictionary<int, int> OrderPositions { get; set; } = new Dictionary<int, int>();
    }
    public class OrderPlacingSagaDefinition : SagaDefinition<OrderPlacingSagaState>
    {
        public OrderPlacingSagaDefinition()
        {
            //ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderPlacingSagaState> sagaConfigurator)
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
            base.ConfigureSaga(endpointConfigurator, sagaConfigurator);
        }
    }
    /// <summary>
    /// Сага размещения заказа
    /// Включает создание записи заказа и резервации продуктов
    /// При успешном выполнение выбрасывает соответствующее событие
    /// </summary>
    public class OrderPlacingSagaStateMachine : MassTransitStateMachine<OrderPlacingSagaState>
    {
        private readonly ILogger<OrderPlacingSagaStateMachine> _logger;
        private Event<OrderPlacingSagaRequest> OnOrderPlacing { get; set; }
        private Event<PaymentPendingCancelledSagaEvent> OnPaymentCancelled { get; set; }
        //private Event<ShipmentPendingCancelledSagaEvent> OnShipmentCancelled { get; set; }
        private Event<OrderPlacingCompletedRSEvent> OnOrderPlacingRSCompleted { get; set; }
        private Event<OrderPlacingFaultedRSEvent> OnOrderPlacingRSFaulted { get; set; }
        

        private State OrderPlacingAwait { get; set; }
        private State OrderPlacingSuccess { get; set; }
        private State OrderPlacingFault { get; set; }
        private State OrderPlacingCompensated { get; set; }

        public OrderPlacingSagaStateMachine(ILogger<OrderPlacingSagaStateMachine> logger)
        {
            _logger = logger;

            Event(() => OnOrderPlacing, e =>
            {
                var initId = Guid.NewGuid();
                //e.CorrelateById(ctx => ctx.Message.OrderID);
                e.InsertOnInitial = true;
                e.SelectId(x => initId);
                e.SetSagaFactory(ctx => new OrderPlacingSagaState()
                {
                    CorrelationId = initId,
                    OrderID = ctx.Message.OrderID,
                    OrderPositions = ctx.Message.Positions,
                    CustomerID = ctx.Message.CustomerID,

                    OrderPlacingRSTrackingNumber = Guid.NewGuid()
                });
            });

            Event(() => OnOrderPlacingRSCompleted, e =>
            {
                e.CorrelateBy((state, ctx) => state.OrderPlacingRSTrackingNumber == ctx.Message.TrackingNumber);
            });

            Event(() => OnOrderPlacingRSFaulted, e =>
            {
                e.CorrelateBy((state, ctx) => state.OrderPlacingRSTrackingNumber == ctx.Message.TrackingNumber);
            });

            Event(() => OnPaymentCancelled, e =>
            {
                e.CorrelateBy((state, ctx) => state.OrderID == ctx.Message.OrderID);
            });

            //Event(() => OnShipmentCancelled, e =>
            //{
            //    e.CorrelateBy((state, ctx) => state.OrderID == ctx.Message.OrderID);
            //});


            /* Событие размещения - запустить RS размещения заказа */
            Initially(
                When(OnOrderPlacing)
                .PublishAsync(ctx => ctx.Init<OrderPlacingRSRequest>(
                    new OrderPlacingRSRequest
                    {
                        TrackingNumber = ctx.Instance.OrderPlacingRSTrackingNumber
                    }))
                .TransitionTo(OrderPlacingAwait));

            /* RS размещения успешно завершен - выбросить событие о завершения саги */
            During(OrderPlacingAwait,
                When(OnOrderPlacingRSCompleted)
                .Then(ctx => {
                    ctx.Instance.OrderID = ctx.Data.OrderID;
                    ctx.Instance.ReserveID = ctx.Data.ReserveID;
                })
                .PublishAsync(ctx => ctx.Init<OrderPlacingSuccessSagaEvent>(
                    new OrderPlacingSuccessSagaEvent
                    {
                        OrderID = ctx.Instance.OrderID,
                        CustomerID = ctx.Instance.CustomerID,
                        Positions = ctx.Instance.OrderPositions
                    }))
                .TransitionTo(OrderPlacingSuccess));

            /* При ошибке RS размещения заказа */
            During(OrderPlacingAwait,
                When(OnOrderPlacingRSFaulted)
                .TransitionTo(OrderPlacingFault));

            /* При отмене отказа платежа - компенсировать резервацию и созданную запись заказа */
            During(OrderPlacingSuccess,
                When(OnPaymentCancelled)
                .PublishAsync(ctx => ctx.Init<ProductsReserveCancelSvcRequest>(
                    new ProductsReserveCancelSvcRequest
                    {
                        ReserveID = ctx.Instance.ReserveID
                    }))
                .PublishAsync(ctx => ctx.Init<OrderCancelSvcRequest>(
                    new OrderCancelSvcRequest
                    {
                        OrderID = ctx.Instance.OrderID
                    }))
                .TransitionTo(OrderPlacingCompensated));


        }
    }
}
