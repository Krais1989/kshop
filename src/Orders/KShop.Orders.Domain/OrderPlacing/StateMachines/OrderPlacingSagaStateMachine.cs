using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.Products;
using KShop.Communications.Contracts.Shipments;
using KShop.Orders.Domain.RoutingSlips.OrderInitialization;
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
        //public Guid OrderID => CorrelationId;
        public Guid? ReserveID { get; set; }
        //public Guid? OrderPlacingRSTrackingNumber { get; set; }

        public int CustomerID { get; set; }
        public int CurrentState { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
    }
    public class OrderPlacingSagaStateMachineDefinition : SagaDefinition<OrderPlacingSagaState>
    {
        public OrderPlacingSagaStateMachineDefinition()
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

        private Event<OrderGetStatusSagaRequest> OnGetStatus { get; set; }

        #region Order
        private State OrderPlacing { get; set; }
        private State OrderPlacingSuccess { get; set; }
        private State OrderPlacingFault { get; set; }

        private Event<OrderPlacingSagaRequest> OnOrderPlacing { get; set; }
        private Event<OrderPlacingCompletedRSEvent> OnOrderPlacingRSCompleted { get; set; }
        private Event<OrderPlacingFaultedRSEvent> OnOrderPlacingRSFaulted { get; set; }
        #endregion

        #region Payment
        private State PaymentProcessing { get; set; }
        private State PaymentProcessingSuccess { get; set; }
        private State PaymentProcessingFault { get; set; }

        private Event OnPaymentSuccessed { get; set; }
        private Event OnPaymentFaulted { get; set; }
        #endregion

        #region Shipment
        private State ShipmentProcessing { get; set; }
        private State ShipmentProcessingSuccess { get; set; }
        private State ShipmentProcessingFault { get; set; }

        private Event OnShipmentSuccessed { get; set; }
        private Event OnShipmentFaulted { get; set; }
        #endregion


        public OrderPlacingSagaStateMachine(ILogger<OrderPlacingSagaStateMachine> logger)
        {
            _logger = logger;

            InstanceState(e => e.CurrentState, OrderPlacing, OrderPlacingSuccess, OrderPlacingFault, OrderPlacingCompensated);

            Event(() => OnOrderPlacing, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
                e.SelectId(ctx => ctx.Message.OrderID);
            });

            Event(() => OnOrderPlacingRSCompleted, e =>
            {
                e.CorrelateById(ctx => ctx.Message.SubmissionID);
            });

            Event(() => OnOrderPlacingRSFaulted, e =>
            {
                e.CorrelateById(ctx => ctx.Message.SubmissionID);
            });

            Event(() => OnPaymentCancelled, e =>
            {
                // TODO: OnPaymentCancelled коррелирует по полю OrderID, возможно стоит переименовать SubmissionID
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            Event(() => OnGetStatus, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            /* Событие размещения - запустить RS размещения заказа */
            Initially(
                When(OnOrderPlacing)
                .ThenAsync(HandleOnOrderPlacing)
                .TransitionTo(OrderPlacing));

            /* RS размещения успешно завершен - выбросить событие о завершения саги */
            During(OrderPlacing,
                When(OnOrderPlacingRSCompleted)
                .ThenAsync(HandleOnOrderPlacingRSCompleted)
                .TransitionTo(OrderPlacingSuccess));

            /* При ошибке RS размещения заказа */
            During(OrderPlacing,
                When(OnOrderPlacingRSFaulted)
                .ThenAsync(HandleOnOrderPlacingRSFaulted)
                .TransitionTo(OrderPlacingFault));

            /* При отмене отказа платежа - компенсировать резервацию и созданную запись заказа */
            During(OrderPlacingSuccess,
                When(OnPaymentCancelled)
                .ThenAsync(HandleOnPaymentCancelled)
                .TransitionTo(OrderPlacingCompensated));

            DuringAny(When(OnGetStatus).ThenAsync(HandleOnGetStatus));
        }

        private async Task HandleOnOrderPlacing(BehaviorContext<OrderPlacingSagaState, OrderPlacingSagaRequest> ctx)
        {
            ctx.Instance.CustomerID = ctx.Data.CustomerID;
            ctx.Instance.OrderPositions = ctx.Data.Positions;
            //ctx.Instance.OrderPlacingRSTrackingNumber = Guid.NewGuid();

            await ctx.Publish(new OrderPlacingRSRequest
            {
                SubmissionID = ctx.Data.OrderID,
                Positions = ctx.Data.Positions,
                CustomerID = ctx.Data.CustomerID
            });
        }

        private async Task HandleOnOrderPlacingRSCompleted(BehaviorContext<OrderPlacingSagaState, OrderPlacingCompletedRSEvent> ctx)
        {
            ctx.Instance.ReserveID = ctx.Data.ReserveID;

            await ctx.Publish(new OrderPlacingSuccessSagaEvent
            {
                OrderID = ctx.Data.OrderID,
                CustomerID = ctx.Instance.CustomerID,
                Positions = ctx.Instance.OrderPositions
            });
        }

        private async Task HandleOnOrderPlacingRSFaulted(BehaviorContext<OrderPlacingSagaState, OrderPlacingFaultedRSEvent> ctx)
        {
            _logger.LogError($"RS OrderPlacing is FAULTED! {ctx.Data.SubmissionID}");
        }

        private async Task HandleOnPaymentCancelled(BehaviorContext<OrderPlacingSagaState, PaymentPendingCancelledSagaEvent> ctx)
        {
            await ctx.Publish(new ProductsReserveCancelSvcRequest
            {
                ReserveID = ctx.Instance.ReserveID.Value
            });

            await ctx.Publish(new OrderCancelSvcRequest
            {
                OrderID = ctx.Data.OrderID
            });
        }

        private async Task HandleOnGetStatus(BehaviorContext<OrderPlacingSagaState, OrderGetStatusSagaRequest> ctx)
        {
            await ctx.RespondAsync(new OrderGetStatusSagaResponse()
            {
                Status = ctx.Instance.CurrentState
            });
        }
    }
}
