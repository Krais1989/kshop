using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Catalogues.UnitTests
{
    [TestFixture]
    public class CreateOrderSataTests
    {
        public async Task SagaInstanceIsCreated()
        {
            
            var services = new ServiceCollection();
            //services.AddLogging()

            var harness = new InMemoryTestHarness();


        }
    }
}
  