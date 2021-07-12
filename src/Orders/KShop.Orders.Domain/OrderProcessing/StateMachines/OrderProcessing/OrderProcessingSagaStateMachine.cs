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
        public int CurrentState { get; set; }
        public int Version { get; set; }

        public Guid CorrelationId { get; set; }


        //public Guid OrderID { get; set; }
        public uint CustomerID { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public Address ShipmentAddress { get; set; }


        public Money OrderPrice { get; set; }


        public Guid? PaymentID { get; set; }
        public Guid? ShipmentID { get; set; }


        /// <summary>
        /// Лог статусов заказа. Используется для компенсации
        /// </summary>
        public List<EOrderStatus> Statuses { get; set; } = new List<EOrderStatus>();
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
    public partial class OrderProcessingSagaStateMachine : MassTransitStateMachine<OrderProcessingSagaState>
    {
        private readonly ILogger<OrderProcessingSagaStateMachine> _logger;

        public State ProcessingCompensation { get; set; }
        public State OrderShipped { get; set; }

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
                        await ctx.Publish(new ProductsReserveCancelSvcRequest(ctx.Instance.CorrelationId, ctx.Instance.CustomerID));
                        break;
                    case EOrderStatus.Created:
                        await ctx.Publish(new OrderCancelSvcRequest(ctx.Instance.CorrelationId, ctx.Instance.CustomerID));
                        break;
                    case EOrderStatus.Payed:
                        await ctx.Publish(new PaymentCancelSvcRequest(ctx.Instance.PaymentID.Value, ctx.Instance.CustomerID));
                        break;
                    case EOrderStatus.Shipped:
                        await ctx.Publish(new ShipmentCancelSvcRequest(ctx.Instance.ShipmentID.Value, ctx.Instance.CustomerID));
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

            await ctx.Publish(new OrderSetStatusCancelledSvcRequest(ctx.Instance.CustomerID, ctx.Instance.CorrelationId, ""));
        }


        public OrderProcessingSagaStateMachine(ILogger<OrderProcessingSagaStateMachine> logger)
        {
            _logger = logger;

            InstanceState(e => e.CurrentState,
                ProductsReservation,
                OrderCreation,
                PaymentProcessing,
                ShipmentProcessing,
                OrderShipped,
                ProcessingCompensation
                );


            ConfigureOrderSubmit();
            ConfigureOrderCreation();
            ConfigureProductsReserving();
            ConfigurePayment();
            ConfigureShipment();
            ConfigureGetStatus();
        }

    }
}
