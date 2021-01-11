using KShop.Catalogues.Domain.Consumers;
using KShop.Catalogues.Persistence;
using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.ValueObjects;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Catalogues.UnitTests
{
    [TestFixture]
    public class TestOrderReservation
    {
        [Test]
        public async Task Should_respond_with_acceptance_if_ok()
        {
            var logMock = new Mock<ILogger<OrderReserveConsumer>>();
            var pubMock = new Mock<IPublishEndpoint>();
            var contextMock = new Mock<CatalogueContext>();


            var harness = new InMemoryTestHarness();
            var reserveConsumer = harness.Consumer(() =>
                new OrderReservationConsumer(logMock.Object, pubMock.Object, contextMock.Object));

            await harness.Start();

            try
            {
                var orderId = NewId.NextGuid();
                var prositions = new List<ProductStack> { new ProductStack { ProductID = 3, Quantity = 5 } };
                await harness.InputQueueSendEndpoint.Send(new OrderReserveEvent
                {
                    OrderID = orderId,
                    Positions = prositions
                });
                await harness.InputQueueSendEndpoint.Send(new OrderPayEvent
                {
                    OrderID = orderId,
                    Price = 100
                });

                Assert.That(reserveConsumer.Consumed.Select<IOrderPayEvent>().Any(), Is.False);
                Assert.That(reserveConsumer.Consumed.Select<IOrderReserveEvent>().Any(), Is.True);
                Assert.That(reserveConsumer.Consumed.Select<IOrderReserveEvent>().First().Context.Message.OrderID, Is.EqualTo(orderId));

                //var s = consumer.Consumed.Select<IOrderReserveEvent>().First().Context.Message.Positions;
                //Assert.That(, Is.True);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
