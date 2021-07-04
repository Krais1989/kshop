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
        private Event<OrderSubmitSagaRequest> OnOrderSubmiting { get; set; }

        private void ConfigureOrderSubmiting()
        {
            Event(() => OnOrderSubmiting, e =>
            {
                e.CorrelateById(ctx => ctx.Message.OrderID);
                e.SelectId(ctx => ctx.Message.OrderID);
                e.InsertOnInitial = true;
            });

            /* Событие размещения - запустить RS размещения заказа */
            Initially(
                When(OnOrderSubmiting)
                .ThenAsync(HandleOnOrderSubmit)
                .TransitionTo(OrderReserving));
        }

        private async Task HandleOnOrderSubmit(BehaviorContext<OrderProcessingSagaState, OrderSubmitSagaRequest> ctx)
        {
            _logger.LogDebug($"Saga - Started - {ctx.Instance.CorrelationId}");

            ctx.Instance.CustomerID = ctx.Data.Customer;
            ctx.Instance.OrderContent = ctx.Data.OrderContent;
            //ctx.Instance.Money = ctx.Data.Price;
            ctx.Instance.PaymentProvider = ctx.Data.PaymentProvider;
            ctx.Instance.ShippingMethod = ctx.Data.ShippingMethod;

            _logger.LogDebug($"Saga - Start Order Reservation");
            await ctx.Publish(new OrderPlacingRSRequest
            {
                OrderID = ctx.Data.OrderID,
                OrderContent = ctx.Data.OrderContent,
                CustomerID = ctx.Data.Customer,
                PaymentProvider = ctx.Data.PaymentProvider,
                // Price = ctx.Data.Price
            });
        }
    }
}
