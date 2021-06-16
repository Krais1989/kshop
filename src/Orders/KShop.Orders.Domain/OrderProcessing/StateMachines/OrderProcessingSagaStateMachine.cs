using Automatonymous;
using GreenPipes;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public class OrderProcessingSagaState : SagaStateMachineInstance, ISagaVersion
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

        /// <summary>
        /// Лог статусов заказа. Используется для компенсации
        /// </summary>
        public List<EOrderStatus> Statuses { get; set; } = new List<EOrderStatus>();
        public int Version { get; set; }
    }
    public class OrderProcessingSagaStateMachineDefinition : SagaDefinition<OrderProcessingSagaState>
    {
        public OrderProcessingSagaStateMachineDefinition()
        {
            //ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderProcessingSagaState> sagaConfigurator)
        {
            //sagaConfigurator.UseMessageRetry(e =>
            //{
            //    e.Intervals(500, 5000, 10000);
            //});

            //sagaConfigurator.UseCircuitBreaker(e =>
            //{
            //    e.ActiveThreshold = 10;
            //    e.TrackingPeriod = TimeSpan.FromSeconds(10);

            //    e.ResetInterval = TimeSpan.FromSeconds(10);
            //    e.TripThreshold = 10;
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
                e.InsertOnInitial = true;
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
            //ctx.Instance.Money = ctx.Data.Price;
            ctx.Instance.PaymentProvider = ctx.Data.PaymentProvider;

            await ctx.Publish(new OrderPlacingRSRequest
            {
                OrderID = ctx.Data.OrderID,
                OrderPositions = ctx.Data.Positions,
                CustomerID = ctx.Data.CustomerID,
                PaymentProvider = ctx.Data.PaymentProvider,
               // Price = ctx.Data.Price
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
            ctx.Instance.Statuses.Add(EOrderStatus.Reserved);
            ctx.Instance.ProductsReserves = ctx.Data.ReservedProducts;
            //TODO: выставить реальную стоимость заказа
            ctx.Instance.Money = new Money(100);

            await ctx.Publish(new PaymentCreateSvcCommand()
            {
                OrderID = ctx.Data.OrderID,
                Money = ctx.Instance.Money,
                PaymentPlatform = ctx.Instance.PaymentProvider
            });

            await ctx.Publish(new OrderSetStatusReservedSvcRequest(ctx.Data.OrderID));
        }

        private async Task HandleOnOrderReserveFault(BehaviorContext<OrderProcessingSagaState, ProductsReserveFaultEvent> ctx)
        {
            _logger.LogError($"RS OrderPlacing is FAULTED! {ctx.Data.OrderID}");

            await Compensate(ctx);
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
                e.CorrelateById(ctx => ctx.Message.OrderID);
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
            ctx.Instance.Statuses.Add(EOrderStatus.Payed);

            await ctx.Publish(new ShipmentCreateSvcCommand()
            {
                OrderID = ctx.Data.OrderID,
                OrderPositions = ctx.Instance.OrderPositions
            });

            await ctx.Publish(new OrderSetStatusPayedSvcRequest(ctx.Data.OrderID));
        }

        private async Task HandleOnPaymentFaulted(BehaviorContext<OrderProcessingSagaState, PaymentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.ErrorMessage);
            // TODO: вызвать компенсацию предыдущих шагов

            await Compensate(ctx);
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
                e.CorrelateById(ctx => ctx.Message.OrderID);
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
            ctx.Instance.Statuses.Add(EOrderStatus.Shipped);

            await ctx.Publish(new OrderSetStatusShippedSvcRequest(ctx.Data.OrderID));
        }

        private async Task HandleOnShipmentFault(BehaviorContext<OrderProcessingSagaState, ShipmentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.ErrorMessage);
            await Compensate(ctx);
        }

        #endregion

        private async Task Compensate<K>(BehaviorContext<OrderProcessingSagaState, K> ctx)
        {
            var statuses = new List<EOrderStatus>(ctx.Instance.Statuses);
            statuses.Reverse();

            foreach (var s in statuses)
            {
                _logger.LogWarning($"Compensate order {ctx.Instance.CorrelationId} - for {s}");
                switch (s)
                {
                    case EOrderStatus.Reserved:
                        await ctx.Publish(new ProductsReserveCancelSvcRequest(ctx.Instance.CorrelationId));
                        break;
                    case EOrderStatus.Payed:
                        await ctx.Publish(new PaymentCancelSvcRequest() { PaymentID = ctx.Instance.PaymentID.Value });
                        break;
                    case EOrderStatus.Shipped:
                        await ctx.Publish(new ShipmentCancelSvcRequest() { ShipmentID = ctx.Instance.ShipmentID.Value });
                        break;
                    case EOrderStatus.Faulted:
                        break;
                    case EOrderStatus.Refunded:
                        break;
                    case EOrderStatus.Cancelled:
                        break;
                    default:
                        break;
                }
            }

            await ctx.Publish(new OrderSetStatusCancelledSvcRequest(ctx.Instance.CorrelationId));
        }


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
