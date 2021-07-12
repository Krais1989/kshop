using Automatonymous;
using KShop.Orders.Persistence;
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
        private State PaymentProcessing { get; set; }

        private Event<PaymentCreateSuccessSvcEvent> OnPaymentSuccessed { get; set; }
        private Event<PaymentCreateFaultSvcEvent> OnPaymentFaulted { get; set; }

        private void ConfigurePayment()
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
                .TransitionTo(ProcessingCompensation));
        }

        private async Task HandleOnPaymentSuccessed(BehaviorContext<OrderProcessingSagaState, PaymentCreateSuccessSvcEvent> ctx)
        {
            _logger.LogDebug($"Saga - Payment Created");

            ctx.Instance.PaymentID = ctx.Data.PaymentID;
            ctx.Instance.Statuses.Add(EOrderStatus.Payed);

            _logger.LogDebug($"Saga - Start Shipment Initialization");
            await ctx.Publish(
                new ShipmentCreateSvcRequest(orderID: ctx.Data.OrderID, orderContent: ctx.Instance.OrderContent, userID: ctx.Instance.CustomerID));

            await ctx.Publish(new OrderSetStatusPayedSvcRequest(ctx.Instance.CustomerID, ctx.Data.OrderID, ""));
        }

        private async Task HandleOnPaymentFaulted(BehaviorContext<OrderProcessingSagaState, PaymentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.ErrorMessage);
            // TODO: вызвать компенсацию предыдущих шагов

            await Compensate(ctx);
        }
    }
}
