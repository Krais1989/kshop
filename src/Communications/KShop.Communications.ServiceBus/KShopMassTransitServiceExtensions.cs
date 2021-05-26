using GreenPipes;
using KShop.ServiceBus;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Pipeline.Filters;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Communications.ServiceBus
{
    public static class KShopMassTransitServiceExtensions
    {
        public static IServiceCollection AddKShopMassTransitRabbitMq(
            this IServiceCollection services,
            IConfiguration config,
            Action<IServiceCollectionBusConfigurator> busServicesConf = null,
            Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> rabbitConf = null)
        {
            var connection_name = Assembly.GetEntryAssembly().FullName;

            var rabbinCon = new RabbitConnection();
            config.GetSection("RabbitConnection").Bind(rabbinCon);

            services.AddMassTransit(x =>
            {
                x.ApplyKShopMassTransitConfiguration();

                if (busServicesConf != null)
                    busServicesConf(x);


                //x.AddConsumers(typeof(PaymentCreateSvcConsumer).Assembly);
                //x.AddRequestClient<PaymentCreateSvcRequest>();
                //x.AddRequestClient<PaymentCancelSvcRequest>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbinCon.HostName, rabbinCon.Port, rabbinCon.VirtualHost, connection_name, host =>
                    {
                        host.Username(rabbinCon.Username);
                        host.Password(rabbinCon.Password);
                    });

                    cfg.KShopApplyBusConfiguration();

                    cfg.KShopTracePublishFilter(ctx);
                    cfg.KShopTraceSendFilter(ctx);
                    cfg.KShopTraceConsumeFilter(ctx);
                    cfg.KShopExecuteConsumeFilter(ctx);
                    cfg.KShopCompensateConsumeFilter(ctx);

                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx);

                    if (rabbitConf != null)
                        rabbitConf(ctx, cfg);


                    cfg.UseMessageRetry(retry =>
                    {
                        // Интервалы в мс: s + i*n, где s - начальный интервал, i - шаг, n - добавочный интервал после каждого шага
                        retry.Incremental(10, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(1000));
                    });

                    cfg.UseScheduledRedelivery(redil =>
                    {
                        redil.Interval(10, TimeSpan.FromMilliseconds(5000));
                    });

                    //cfg.UseInMemoryOutbox(outbox => { 
                    //    outbox.ConcurrentMessageDelivery
                    //});
                });

            }).AddMassTransitHostedService();

            return services;
        }
    }

}
