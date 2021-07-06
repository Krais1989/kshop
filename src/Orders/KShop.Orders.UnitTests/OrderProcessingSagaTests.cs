using KShop.Orders.Domain;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Orders.UnitTests
{
    [TestFixture]
    public class OrderProcessingSagaTests
    {
        [Test]
        public async Task Test1()
        {
            var mock_logger = new Mock<ILogger<OrderProcessingSagaStateMachine>>();
            var statemachine_order_processing = new OrderProcessingSagaStateMachine(mock_logger.Object);


            var harness = new InMemoryTestHarness();
            var harness_order_processing 
                = harness.StateMachineSaga<OrderProcessingSagaState, OrderProcessingSagaStateMachine>(statemachine_order_processing);

            await harness.Start();

            try
            {
                var submitOrder = new OrderSubmitSagaRequest
                {
                    OrderID = Guid.NewGuid(),
                    Address = new Address { Data = "Test address" },
                    CustomerID = 1,
                    OrderContent = new List<ProductQuantity> { },
                    PaymentProvider = EPaymentProvider.Mock,
                    ShippingMethod = EShippingMethod.Default
                };
                await harness.Bus.Publish(submitOrder);

                Assert.IsTrue(harness.Consumed.Select<OrderSubmitSagaRequest>().Any());
                Assert.IsTrue(harness_order_processing.Consumed.Select<OrderSubmitSagaRequest>().Any());

                var result = harness_order_processing.Created.Contains(submitOrder.OrderID);
                var instance = harness_order_processing.Created.ContainsInState(submitOrder.OrderID, statemachine_order_processing, statemachine_order_processing.ProductsReservation);
                Assert.IsNotNull(instance, $"Saga not in state {nameof(statemachine_order_processing.ProductsReservation)}");
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
