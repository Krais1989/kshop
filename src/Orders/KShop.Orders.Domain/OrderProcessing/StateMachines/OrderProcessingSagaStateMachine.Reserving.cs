using Automatonymous;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public partial class OrderProcessingSagaStateMachine
    {
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
            _logger.LogDebug($"Saga - Reserve Completed");

            ctx.Instance.Statuses.Add(EOrderStatus.Reserved);
            ctx.Instance.ProductsReserves = ctx.Data.ReservedProducts;
            //TODO: выставить реальную стоимость заказа
            ctx.Instance.Money = new Money(100);

            _logger.LogDebug($"Saga - Start Payment Inialization");
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
            _logger.LogDebug($"Saga - Reserve Faulted");


            await Compensate(ctx);
        }
    }
}
