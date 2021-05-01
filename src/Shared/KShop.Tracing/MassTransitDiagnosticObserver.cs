//using MassTransit;
//using MassTransit.Context;
//using Microsoft.Extensions.DiagnosticAdapter;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Threading;

//namespace KShop.Tracing
//{
//    public sealed class MassTransitDiagnosticObserver
//        : IObserver<DiagnosticListener>
//        , IObserver<KeyValuePair<string, object>>
//    {
//        private readonly ILogger<MassTransitDiagnosticObserver> _logger;
//        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
//        private readonly AsyncLocal<Stopwatch> _stopwatch = new AsyncLocal<Stopwatch>();

//        public MassTransitDiagnosticObserver(ILogger<MassTransitDiagnosticObserver> logger)
//        {
//            _logger = logger;
//        }

//        public void OnCompleted()
//        {
//            _logger.LogInformation("Complete");
//            _subscriptions.ForEach(e => e.Dispose());
//            _subscriptions.Clear();
//        }

//        public void OnError(Exception error)
//        {
//            _logger.LogError($"Error: {error.Message}");
//        }

//        /// <summary>
//        /// Вызывается единожды для каждого созданного DiagnosticListener
//        /// </summary>
//        /// <param name="listener"></param>
//        public void OnNext(DiagnosticListener listener)
//        {
//            if (listener.Name == "MassTransit")
//            {
//                _logger.LogWarning($"OnNext: {listener.Name}");
//                //var subscription = listener.Subscribe(this, IsEnabled);
//                var subscription = listener.SubscribeWithAdapter(this);
//                _subscriptions.Add(subscription);
//            }
//        }

//        /// <summary>
//        /// Callback для зарегистрированных событий MassTransit
//        /// </summary>
//        /// <param name="value"></param>
//        public void OnNext(KeyValuePair<string, object> value)
//        {
//            var ctx = (SendContext)value.Value;
//            _logger.LogWarning(value.Key);
//        }

//        [DiagnosticName("MassTransit.Transport.Send.Start")]
//        public void OnTransportSendStart(SendContext context)
//        {
//            _logger.LogWarning("OnTransportSendStart");
//            if (context != null)
//            {
//                _logger.LogWarning(context.InitiatorId.Value.ToString());
//            }
            
//        }
//        [DiagnosticName("MassTransit.Transport.Send.Stop")]
//        public void OnTransportSendStop()
//        {
//            _logger.LogWarning("OnTransportSendStop");

//        }

//        private static bool IsEnabled(string name)
//        {
//            return name == "MassTransit.Transport.Send.Start" || name == "MassTransit.Transport.Send.Stop"
//            || name == "MassTransit.Transport.Receive.Start" || name == "MassTransit.Transport.Receive.Stop"
//            || name == "MassTransit.Consumer.Consume.Start" || name == "MassTransit.Consumer.Consume.Stop"
//            || name == "MassTransit.Consumer.Handle.Start" || name == "MassTransit.Consumer.Handle.Stop"

//            || name == "MassTransit.Saga.Send.Start" || name == "MassTransit.Saga.Send.Stop"
//            || name == "MassTransit.Saga.SendQuery.Start" || name == "MassTransit.Saga.SendQuery.Stop"
//            || name == "MassTransit.Saga.Initiate.Start" || name == "MassTransit.Saga.Initiate.Stop"
//            || name == "MassTransit.Saga.Orchestrate.Start" || name == "MassTransit.Saga.Orchestrate.Stop"
//            || name == "MassTransit.Saga.Observe.Start" || name == "MassTransit.Saga.Observe.Stop"
//            || name == "MassTransit.Saga.RaiseEvent.Start" || name == "MassTransit.Saga.RaiseEvent.Stop"

//            || name == "MassTransit.Activity.Execute.Start" || name == "MassTransit.Activity.Execute.Stop"
//            || name == "MassTransit.Activity.Compensate.Start" || name == "MassTransit.Activity.Compensate.Stop";
//        }
//    }

//}
