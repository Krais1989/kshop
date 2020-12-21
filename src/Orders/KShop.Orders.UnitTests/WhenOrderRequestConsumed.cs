using System;
using System.Threading.Tasks;
using MassTransit.Testing;
using NUnit.Framework;

namespace KShop.Orders.UnitTests
{

    [TestFixture]
    public class WhenOrderRequestConsumed
    {
        public async Task Test1()
        {
            var harness = new InMemoryTestHarness();
            //var consumer = harness.Consumer<OrderReserveConsumer>

        }
    }
}
