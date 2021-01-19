using MassTransit;
using MassTransit.Contracts.Conductor;
using MassTransit.Internals.Extensions;
using MassTransit.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KShop.Communications.ServiceBus
{
    public static class KShopMasstransitNameFormatterConfigurationExtensions
    {
        public static void SetKShopEndpointsNameFormatter(this IBusFactoryConfigurator configurator)
        {
            var defaultNameFormatter = configurator.MessageTopology.EntityNameFormatter;
            configurator.MessageTopology.SetEntityNameFormatter(new KShopMassTransitNameFormatter(defaultNameFormatter));
        }
    }

    /// <summary>
    /// Кастомный форматтер имен для MassTransit
    /// </summary>
    public class KShopMassTransitNameFormatter : IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _entityNameFormatter;

        public KShopMassTransitNameFormatter(IEntityNameFormatter entityNameFormatter)
        {
            _entityNameFormatter = entityNameFormatter;
        }

        public string FormatEntityName<T>()
        {
            if (typeof(T).ClosesType(typeof(Link<>), out Type[] types)
                || typeof(T).ClosesType(typeof(Up<>), out types)
                || typeof(T).ClosesType(typeof(Down<>), out types)
                || typeof(T).ClosesType(typeof(Unlink<>), out types))
            {
                var name = (string)typeof(IEntityNameFormatter)
                    .GetMethod("FormatEntityName")
                    .MakeGenericMethod(types)
                    .Invoke(_entityNameFormatter, Array.Empty<object>());

                var suffix = typeof(T).Name.Split('`').First();
                return $"{name}-{suffix}";
            }

            return _entityNameFormatter.FormatEntityName<T>();
        }
    }
}
