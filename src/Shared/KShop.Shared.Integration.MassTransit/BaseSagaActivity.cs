using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace KShop.Shared.Integration.MassTransit
{
    public class BaseSagaActivity<TState, TEvent> : Activity<TState, TEvent>
    {
        protected readonly ILogger _logger;

        public BaseSagaActivity(ILogger logger)
        {
            _logger = logger;
        }

        protected virtual string ScopeName => GetType().Name;

        public virtual void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual async Task Execute(BehaviorContext<TState, TEvent> context, Behavior<TState, TEvent> next)
        {
            _logger.LogInformation($"{context.Data.GetType().Name}: {JsonSerializer.Serialize(context.Data)}");
        }

        public virtual Task Faulted<TException>(BehaviorExceptionContext<TState, TEvent, TException> context, Behavior<TState, TEvent> next) where TException : Exception
        {
            _logger.LogError($"FAULTED: {context.Exception.Message}");
            return next.Faulted(context);
        }

        public virtual void Probe(ProbeContext context)
        {
            context.CreateScope(ScopeName);
        }
    }
}
