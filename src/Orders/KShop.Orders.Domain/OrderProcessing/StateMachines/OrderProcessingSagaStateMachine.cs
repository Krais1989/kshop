using Automatonymous;
using GreenPipes;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.Products;
using KShop.Communications.Contracts.Shipments;
using KShop.Communications.Contracts.ValueObjects;
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
    public class OrderProcessingSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        //public Guid OrderID => CorrelationId;
        public ProductsReserveMap ProductsReserves { get; set; }
        //public Guid? OrderPlacingRSTrackingNumber { get; set; }
        public OrderPositionsMap OrderPositions { get; set; }
        public Money Money { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }

        public Guid? PaymentID { get; set; }
        public Guid? ShipmentID { get; set; }

        public int CustomerID { get; set; }
        public int CurrentState { get; set; }

    }
    public class OrderProcessingSagaStateMachineDefinition : SagaDefinition<OrderProcessingSagaState>
    {
        public OrderProcessingSagaStateMachineDefinition()
        {
            //ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderProcessingSagaState> sagaConfigurator)
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
    public class OrderProcessingSagaStateMachine : MassTransitStateMachine<OrderProcessingSagaState>
    {
        private readonly ILogger<OrderProcessingSagaStateMachine> _logger;

        #region Order Get Status
        private Event<OrderGetStatusSagaRequest> OnGetStatus { get; set; }
        void ConfigureOrderGetStatus()
        {
            Event(() => OnGetStatus, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            DuringAny(When(OnGetStatus).ThenAsync(HandleOnGetStatus));
        }
        private async Task HandleOnGetStatus(BehaviorContext<OrderProcessingSagaState, OrderGetStatusSagaRequest> ctx)
        {
            await ctx.RespondAsync(new OrderGetStatusSagaResponse()
            {
                Status = ctx.Instance.CurrentState
            });
        }
        #endregion

        #region Order Placing 
        private Event<OrderPlacingSagaRequest> OnOrderPlacing { get; set; }

        private void ConfigureOrderPlacing()
        {
            Event(() => OnOrderPlacing, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
                e.SelectId(ctx => ctx.Message.OrderID);
            });

            /* Событие размещения - запустить RS размещения заказа */
            Initially(
                When(OnOrderPlacing)
                .ThenAsync(HandleOnOrderPlacing)
                .TransitionTo(OrderReserving));
        }

        private async Task HandleOnOrderPlacing(BehaviorContext<OrderProcessingSagaState, OrderPlacingSagaRequest> ctx)
        {
            ctx.Instance.CustomerID = ctx.Data.CustomerID;
            ctx.Instance.OrderPositions = ctx.Data.Positions;
            ctx.Instance.Money = ctx.Data.Price;
            ctx.Instance.PaymentProvider = ctx.Data.PaymentProvider;

            //ctx.Instance.OrderPlacingRSTrackingNumber = Guid.NewGuid();

            await ctx.Publish(new OrderPlacingRSRequest
            {
                OrderID = ctx.Data.OrderID,
                OrderPositions = ctx.Data.Positions,
                CustomerID = ctx.Data.CustomerID,
                PaymentProvider = ctx.Data.PaymentProvider,
                Price = ctx.Data.Price
            });
        }
        #endregion

        #region Order Reserving
        private State OrderReserving { get; set; }
        private State OrderReservingSuccess { get; set; }
        private State OrderReservingFault { get; set; }


        //private Event<OrderReservingCompletedRSEvent> OnOrderReservingRSCompleted { get; set; }
        // NOTE: при ошибке RS выбрасывается OnOrderReserveFault
        //private Event<OrderReservingFaultedRSEvent> OnOrderReservingRSFaulted { get; set; }

        private Event<ProductsReserveSuccessEvent> OnOrderReserved { get; set; }
        private Event<ProductsReserveFaultEvent> OnOrderReserveFault { get; set; }

        private void ConfigureOrderReserving()
        {
            Event(() => OnOrderReserved, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            Event(() => OnOrderReserveFault, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            /* RS размещения успешно завершен - выбросить событие о завершения саги */
            During(OrderReserving,
                When(OnOrderReserved)
                .ThenAsync(HandleOnOrderReserved)
                .TransitionTo(PaymentProcessing));

            /* При ошибке RS размещения заказа */
            During(OrderReserving,
                When(OnOrderReserveFault)
                .ThenAsync(HandleOnOrderReserveFault)
                .TransitionTo(OrderReservingFault));
        }

        private async Task HandleOnOrderReserved(BehaviorContext<OrderProcessingSagaState, ProductsReserveSuccessEvent> ctx)
        {
            ctx.Instance.ProductsReserves = ctx.Data.ReservedProducts;

            await ctx.Publish(new PaymentCreateSvcCommand()
            {
                OrderID = ctx.Data.OrderID,
                Money = ctx.Instance.Money,
                PaymentPlatform = ctx.Instance.PaymentProvider
            });
        }

        private async Task HandleOnOrderReserveFault(BehaviorContext<OrderProcessingSagaState, ProductsReserveFaultEvent> ctx)
        {
            _logger.LogError($"RS OrderPlacing is FAULTED! {ctx.Data.OrderID}");
        }

        private async Task CompensateOrderReserving(BehaviorContext<OrderProcessingSagaState, PaymentPendingCancelledSagaEvent> ctx)
        {
            await ctx.Publish(new ProductsReserveCancelSvcRequest(ctx.Data.OrderID));
            await ctx.Publish(new OrderCancelSvcRequest(ctx.Data.OrderID));
        }
        #endregion

        #region Order Payment
        private State PaymentProcessing { get; set; }
        private State PaymentProcessingSuccess { get; set; }
        private State PaymentProcessingFault { get; set; }

        private Event<PaymentCreateSuccessSvcEvent> OnPaymentSuccessed { get; set; }
        private Event<PaymentCreateFaultSvcEvent> OnPaymentFaulted { get; set; }

        private void ConfigureOrderPayment()
        {
            Event(() => OnPaymentSuccessed, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });
            Event(() => OnPaymentFaulted, e =>
            {
                e.CorrelateById(ctx => ctx.CorrelationId.Value);
            });

            During(PaymentProcessing,
                When(OnPaymentSuccessed)
                .ThenAsync(HandleOnPaymentSuccessed)
                .TransitionTo(ShipmentProcessing));

            During(PaymentProcessing,
                When(OnPaymentFaulted)
                .ThenAsync(HandleOnPaymentFaulted)
                .TransitionTo(PaymentProcessingFault));
        }

        private async Task HandleOnPaymentSuccessed(BehaviorContext<OrderProcessingSagaState, PaymentCreateSuccessSvcEvent> ctx)
        {
            ctx.Instance.PaymentID = ctx.Data.PaymentID;

            await ctx.Publish(new ShipmentCreateSvcCommand()
            {
                OrderID = ctx.Data.OrderID,
                OrderPositions = ctx.Instance.OrderPositions
            });
        }

        private async Task HandleOnPaymentFaulted(BehaviorContext<OrderProcessingSagaState, PaymentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.Exception, ctx.Data.Exception.Message);
            // TODO: вызвать компенсацию предыдущих шагов
        }
        #endregion

        #region Shipment
        private State ShipmentProcessing { get; set; }
        private State ShipmentProcessingSuccess { get; set; }
        private State ShipmentProcessingFault { get; set; }

        private Event<ShipmentCreateSuccessSvcEvent> OnShipmentSuccessed { get; set; }
        private Event<ShipmentCreateFaultSvcEvent> OnShipmentFault { get; set; }

        private void ConfigureOrderShipment()
        {
            Event(() => OnShipmentSuccessed, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });
            Event(() => OnShipmentFault, e =>
            {
                e.CorrelateById(ctx => ctx.CorrelationId.Value);
            });

            During(ShipmentProcessing,
                When(OnShipmentSuccessed)
                .ThenAsync(HandleOnOnShipmentSuccessed)
                .TransitionTo(ShipmentProcessingSuccess));

            During(ShipmentProcessing,
                When(OnShipmentFault)
                .ThenAsync(HandleOnShipmentFault)
                .TransitionTo(ShipmentProcessingFault));
        }

        private async Task HandleOnOnShipmentSuccessed(BehaviorContext<OrderProcessingSagaState, ShipmentCreateSuccessSvcEvent> ctx)
        {
            ctx.Instance.ShipmentID = ctx.Data.ShipmentID;
        }

        private async Task HandleOnShipmentFault(BehaviorContext<OrderProcessingSagaState, ShipmentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.Exception, ctx.Data.Exception.Message);
            // TODO: вызвать компенсацию предыдущих шагов
        }

        #endregion




        public OrderProcessingSagaStateMachine(ILogger<OrderProcessingSagaStateMachine> logger)
        {
            _logger = logger;

            InstanceState(e => e.CurrentState,
                OrderReserving,
                OrderReservingSuccess,
                OrderReservingFault,

                PaymentProcessing,
                PaymentProcessingSuccess,
                PaymentProcessingFault,

                ShipmentProcessing,
                ShipmentProcessingSuccess,
                ShipmentProcessingFault);

            ConfigureOrderPlacing();
            ConfigureOrderReserving();
            ConfigureOrderPayment();
            ConfigureOrderShipment();
            ConfigureOrderGetStatus();

        }

    }
}
