using System;
using System.Threading.Tasks;
using KShop.Orders.Domain.Sagas;
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
            //var saga = harness.StateMachineSaga<OrderSagaStateMachine>();
            //var consumer = harness.Consumer<OrderReserveConsumer>

        }
    }
}
