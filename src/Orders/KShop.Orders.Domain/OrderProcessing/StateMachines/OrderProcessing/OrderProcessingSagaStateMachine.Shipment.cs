﻿using Automatonymous;
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
        private State ShipmentProcessing { get; set; }

        private Event<ShipmentCreateSuccessSvcEvent> OnShipmentSuccessed { get; set; }
        private Event<ShipmentCreateFaultSvcEvent> OnShipmentFault { get; set; }

        private void ConfigureShipment()
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
                .TransitionTo(OrderShipped));

            During(ShipmentProcessing,
                When(OnShipmentFault)
                .ThenAsync(HandleOnShipmentFault)
                .TransitionTo(ProcessingCompensation));
        }

        private async Task HandleOnOnShipmentSuccessed(BehaviorContext<OrderProcessingSagaState, ShipmentCreateSuccessSvcEvent> ctx)
        {
            ctx.Instance.ShipmentID = ctx.Data.ShipmentID;
            ctx.Instance.Statuses.Add(EOrderStatus.Shipped);

            await ctx.Publish(new OrderSetStatusShippedSvcRequest(ctx.Instance.CustomerID, ctx.Data.OrderID, ""));
        }

        private async Task HandleOnShipmentFault(BehaviorContext<OrderProcessingSagaState, ShipmentCreateFaultSvcEvent> ctx)
        {
            _logger.LogError(ctx.Data.ErrorMessage);
            await Compensate(ctx);
        }
    }
}
