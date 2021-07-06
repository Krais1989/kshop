using Automatonymous;
using KShop.Orders.Persistence;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
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
        public State ProductsReservation { get; set; }
        //private State ProductsReservationSuccess { get; set; }
        //private State ProductsReservationFault { get; set; }


        private Event<ProductsReserveSuccessEvent> OnProductsReserveSuccessEvent { get; set; }
        private Event<ProductsReserveFaultEvent> OnProductsReserveFaultEvent { get; set; }
        //private Event<Fault<ProductsReserveSvcRequest>> OnProductsReserveExceptionEvent { get; set; }

        private void ConfigureProductsReserving()
        {
            Event(() => OnProductsReserveSuccessEvent, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            Event(() => OnProductsReserveFaultEvent, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
            });

            //Event(() => OnProductsReserveExceptionEvent, e =>
            //{
            //    e.CorrelateById(ctx => ctx.Message.Message.OrderID);
            //});

            /* RS размещения успешно завершен - выбросить событие о завершения саги */
            During(ProductsReservation,
                When(OnProductsReserveSuccessEvent)
                .ThenAsync(HandleOnProductsReserved)
                .TransitionTo(OrderCreation));

            /* При ошибке RS размещения заказа */
            /* TODO: добавить в when Fault<ProductsReserveSvcRequest> */
            During(ProductsReservation,
                When(OnProductsReserveFaultEvent)
                .ThenAsync(HandlerOnProductsReserveFault)
                .TransitionTo(ProcessingCompensation));
        }

        private async Task HandleOnProductsReserved(BehaviorContext<OrderProcessingSagaState, ProductsReserveSuccessEvent> ctx)
        {
            _logger.LogDebug($"Saga - Products reserve completed");

            ctx.Instance.Statuses.Add(EOrderStatus.Reserved);
            ctx.Instance.OrderContent = ctx.Data.OrderContent;
            ctx.Instance.OrderPrice = ctx.Data.Price;

            _logger.LogDebug($"Saga - Start Order Inialization");

            await ctx.Publish(new OrderCreateSvcRequest { 
                OrderID = ctx.Instance.CorrelationId,
                CustomerID = ctx.Instance.CustomerID,
                OrderContent = ctx.Instance.OrderContent,
                OrderPrice = ctx.Instance.OrderPrice
            });
        }

        private async Task HandlerOnProductsReserveFault(BehaviorContext<OrderProcessingSagaState, ProductsReserveFaultEvent> ctx)
        {
            _logger.LogDebug($"Saga - Products reserve Faulted");
            await Compensate(ctx);
        }
    }
}
