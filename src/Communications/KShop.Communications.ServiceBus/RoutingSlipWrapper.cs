using Automatonymous;
using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace KShop.Orders.Domain.RoutingSlips.OrderPlacement
{
    public class RoutingSlipWrapperState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentActivity { get; set; }
        public int CurrentState { get; set; }
    }

    public class RoutingSlipCreate_SagaRequest
    {
        public Guid TrackingNumber { get; set; }

        public string OnCreateUri { get; set; }

        public string OnCompleteUri { get; set; }
        public string OnCompleteActivityUri { get; set; }

        public string OnFaultUri { get; set; }
        public string OnFaultActivityUri { get; set; }
    }

    public class RoutingSlipGetStatus_SagaRequest
    {
        public Guid TrackingNumber { get; set; }
    }

    public class GetRoutingSlipStatus_SagaResponse
    {
        public Guid TrackingNumber { get; set; }

        public int CurrentState { get; set; }
        public string CurrentActivity { get; set; }
    }

    public class RoutingSlipWrapper : MassTransitStateMachine<RoutingSlipWrapperState>
    {
        private readonly ILogger<RoutingSlipWrapper> _logger;

        public Event<RoutingSlipCreate_SagaRequest> RoutingSlipCreate { get; private set; }
        public Event<RoutingSlipGetStatus_SagaRequest> RoutingSlipGetStatus { get; private set; }
        public Event<RoutingSlipCompleted> RoutingSlipCompleted_Event { get; private set; }
        public Event<RoutingSlipFaulted> RoutingSlipFaulted_Event { get; private set; }

        public State Created;
        public State Processing;

        public RoutingSlipWrapper(ILogger<RoutingSlipWrapper> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState, Created);

            Event(() => RoutingSlipCreate, x =>
            {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
                x.InsertOnInitial = true;
                x.SelectId(ctx => ctx.Message.TrackingNumber);
                x.SetSagaFactory(ctx => new RoutingSlipWrapperState() { CorrelationId = ctx.Message.TrackingNumber });
            });

            Event(() => RoutingSlipGetStatus, x => {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
                x.OnMissingInstance(cfg => 
                    cfg.ExecuteAsync(ctx => 
                        ctx.RespondAsync(new GetRoutingSlipStatus_SagaResponse()
                        {
                            TrackingNumber = ctx.Message.TrackingNumber,
                            CurrentState = -1,
                            CurrentActivity = null
                        })
                    )
                );
            });

            Event(() => RoutingSlipCompleted_Event, x =>
            {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
            });

            Event(() => RoutingSlipFaulted_Event, x =>
            {
                x.CorrelateById(ctx => ctx.Message.TrackingNumber);
            });

            /* Получение состояния Routing Slip */
            DuringAny(
                When(RoutingSlipGetStatus)
                    .RespondAsync(async x => 
                        await x.Init<GetRoutingSlipStatus_SagaResponse>(new GetRoutingSlipStatus_SagaResponse() { 
                            TrackingNumber = x.Instance.CorrelationId,
                            CurrentActivity = x.Instance.CurrentActivity,
                            CurrentState = x.Instance.CurrentState
                        })
                    )
            );

            Initially(
                When(RoutingSlipCreate)
                    .ThenAsync(async x => { 
                        
                    })
                    .TransitionTo(Created));



        }
    }
}
