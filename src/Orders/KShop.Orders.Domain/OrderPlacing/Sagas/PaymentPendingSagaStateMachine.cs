using Automatonymous;
using KShop.Communications.Contracts.Orders;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KShop.Orders.Domain.Orchestrations
{
    public class PaymentPendingSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public IDictionary<int, int> OrderPositions { get; set; } = new Dictionary<int, int>();
    }

    public class PaymentPendingSagaDefinition : SagaDefinition<PaymentPendingSagaState>
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
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<PaymentPendingSagaState> sagaConfigurator)
        {
            base.ConfigureSaga(endpointConfigurator, sagaConfigurator);
        }
    }

    /// <summary>
    /// Сага оплаты заказа
    /// Включает создание записи оплаты, отправку запроса во внешнюю систему и ожидание ответа
    /// Инициализируется при событии успешного размещения заказа
    /// При успешной оплате выбрасывает соответствующее событие
    /// </summary>
    public class PaymentPendingSagaStateMachine : MassTransitStateMachine<PaymentPendingSagaState>
    {
        public Event<OrderPlacingSuccessSagaEvent> OnOrderPlacingSuccess { get; set; }

        private readonly ILogger<PaymentPendingSagaStateMachine> _logger;

        public PaymentPendingSagaStateMachine(ILogger<PaymentPendingSagaStateMachine> logger)
        {
            _logger = logger;

            Event(() => OnOrderPlacingSuccess, e =>
            {
                var initId = Guid.NewGuid();
                //e.CorrelateById(ctx => ctx.Message.OrderID);
                e.InsertOnInitial = true;
                e.SelectId(x => initId);
                e.SetSagaFactory(ctx => new PaymentPendingSagaState()
                {
                    CorrelationId = initId,
                    OrderID = ctx.Message.OrderID,
                    OrderPositions = ctx.Message.Positions,
                    CustomerID = ctx.Message.CustomerID
                });
            });
        }


    }
}
