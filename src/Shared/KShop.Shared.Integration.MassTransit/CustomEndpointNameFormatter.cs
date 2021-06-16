using MassTransit;
using MassTransit.Definition;
using MassTransit.Topology;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Integration.MassTransit
{
    public static class CustomConfigurationExtensions
    {
        /// <summary>
        /// Конфигурация MassTransit
        /// </summary>
        public static void ApplyKShopMassTransitConfiguration(this IBusRegistrationConfigurator conf)
        {
            conf.SetEndpointNameFormatter(new CustomEndpointNameFormatter());
        }

        /// <summary>
        /// Конфигурация шины
        /// </summary>
        /// <param name="conf"></param>
        public static void KShopApplyBusConfiguration(this IBusFactoryConfigurator conf)
        {
            var entityNameFormatter = conf.MessageTopology.EntityNameFormatter;
            conf.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter(entityNameFormatter));
        }
    }

    /// <summary>
    /// Форматирование имён сообщений
    /// </summary>
    public class CustomEntityNameFormatter : IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _default;

        public CustomEntityNameFormatter(IEntityNameFormatter def)
        {
            _default = def;
        }

        public string FormatEntityName<T>()
        {
            return _default.FormatEntityName<T>();
        }
    }

    /// <summary>
    /// Форматирование имён эндпоинтов
    /// </summary>
    public class CustomEndpointNameFormatter : IEndpointNameFormatter
    {
        private IEndpointNameFormatter _default;

        public CustomEndpointNameFormatter()
        {
            _default = KebabCaseEndpointNameFormatter.Instance;
        }

        public string SanitizeName(string name)
        {
            return _default.SanitizeName(name);
        }

        public string Message<T>() where T : class
        {
            return _default.Message<T>();
        }

        public string TemporaryEndpoint(string tag)
        {
            return _default.TemporaryEndpoint(tag);
        }

        string IEndpointNameFormatter.CompensateActivity<T, TLog>()
        {
            var name = _default.CompensateActivity<T, TLog>();
            return SanitizeName(name);
        }

        string IEndpointNameFormatter.Consumer<T>()
        {
            var name = _default.Consumer<T>();
            return SanitizeName(name);
        }

        string IEndpointNameFormatter.ExecuteActivity<T, TArguments>()
        {
            var name = _default.ExecuteActivity<T, TArguments>();
            return SanitizeName(name);
        }

        string IEndpointNameFormatter.Saga<T>()
        {
            var name = _default.Saga<T>();
            return SanitizeName(name);
        }
    }
}
