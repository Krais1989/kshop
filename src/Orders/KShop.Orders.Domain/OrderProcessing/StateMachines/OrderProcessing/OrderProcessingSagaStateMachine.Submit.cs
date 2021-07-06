using Automatonymous;
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
        private Event<OrderSubmitSagaRequest> OnOrderSubmit { get; set; }

        private void ConfigureOrderSubmit()
        {
            Event(() => OnOrderSubmit, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
                e.SelectId(ctx => ctx.Message.OrderID);
                e.InsertOnInitial = true;
            });

            /* Событие размещения - запустить RS размещения заказа */
            Initially(
                When(OnOrderSubmit)
                .ThenAsync(HandleOnOrderSubmit)
                .TransitionTo(ProductsReservation));
        }

        private async Task HandleOnOrderSubmit(BehaviorContext<OrderProcessingSagaState, OrderSubmitSagaRequest> ctx)
        {
            _logger.LogDebug($"Saga - Started - {ctx.Instance.CorrelationId}");

            ctx.Instance.CustomerID = ctx.Data.CustomerID;
            ctx.Instance.OrderContent = ctx.Data.OrderContent;
            ctx.Instance.PaymentProvider = ctx.Data.PaymentProvider;
            ctx.Instance.ShippingMethod = ctx.Data.ShippingMethod;
            ctx.Instance.ShipmentAddress = ctx.Data.Address;

            _logger.LogDebug($"Saga - Start Order Reservation");
            //await ctx.Publish(new OrderPlacingRSRequest
            //{
            //    OrderID = ctx.Data.OrderID,
            //    OrderContent = ctx.Data.OrderContent,
            //    CustomerID = ctx.Data.CustomerID,
            //    PaymentProvider = ctx.Data.PaymentProvider,
            //    // Price = ctx.Data.Price
            //});

            await ctx.Publish(new ProductsReserveSvcRequest
            {
                CustomerID = ctx.Data.CustomerID,
                OrderID = ctx.Data.OrderID,
                OrderContent = ctx.Data.OrderContent
            });
        }
    }
}
