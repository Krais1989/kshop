using Automatonymous;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    /// <summary>
    /// Order creation
    /// </summary>
    public partial class OrderProcessingSagaStateMachine
    {
        private State OrderCreation { get; set; }

        private Event<OrderCreateSuccessSvcEvent> OnOrderCreationSuccess { get; set; }
        private Event<OrderCreateFaultSvcEvent> OnOrderCreationFault { get; set; }

        private void ConfigureOrderCreation()
        {
            Event(() => OnOrderCreationSuccess, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            Event(() => OnOrderCreationFault, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            /* RS размещения успешно завершен - выбросить событие о завершения саги */
            During(OrderCreation,
                When(OnOrderCreationSuccess)
                .ThenAsync(HandlerOnOrderCreationSuccess)
                .TransitionTo(PaymentProcessing));

            /* При ошибке RS размещения заказа */
            During(OrderCreation,
                When(OnOrderCreationFault)
                .ThenAsync(HandleOnOrderCreationFault)
                .TransitionTo(ProcessingCompensation));
        }

        private async Task HandlerOnOrderCreationSuccess(BehaviorContext<OrderProcessingSagaState, OrderCreateSuccessSvcEvent> ctx)
        {
            _logger.LogDebug($"Saga - Reserve Completed");

            ctx.Instance.Statuses.Add(EOrderStatus.Created);

            _logger.LogDebug($"Saga - Start Payment Inialization");
            
            await ctx.Publish(new PaymentCreateSvcRequest()
            {
                OrderID = ctx.Data.OrderID,
                Money = ctx.Instance.OrderPrice,
                PaymentPlatform = ctx.Instance.PaymentProvider
            });

        }

        private async Task HandleOnOrderCreationFault(BehaviorContext<OrderProcessingSagaState, OrderCreateFaultSvcEvent> ctx)
        {
            _logger.LogDebug($"Saga - Reserve Faulted");
            await Compensate(ctx);
        }
    }
}
