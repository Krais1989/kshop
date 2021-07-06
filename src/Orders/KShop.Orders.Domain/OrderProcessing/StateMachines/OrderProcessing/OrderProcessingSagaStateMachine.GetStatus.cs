using Automatonymous;
using KShop.Shared.Integration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.Domain
{
    public partial class OrderProcessingSagaStateMachine
    {
        private Event<OrderGetStatusSagaRequest> OnGetStatus { get; set; }
        void ConfigureGetStatus()
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
    }
}
