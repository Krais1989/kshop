using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
using KShop.Communications.Contracts.ValueObjects;
using KShop.Orders.Domain.Consumers;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KShop.Orders.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTestController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<OrderPlacingSagaRequest> _createOrderClient;
        private readonly OrderContext _orderContext;

        public OrderTestController(
            IPublishEndpoint publishEndpoint,
            IRequestClient<OrderPlacingSagaRequest> createOrderClient,
            OrderContext orderContext)
        {
            _publishEndpoint = publishEndpoint;
            _createOrderClient = createOrderClient;
            _orderContext = orderContext;
        }


        // GET api/<OrderTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderTestController>
        [HttpGet("[action]")]
        public async ValueTask<IActionResult> PostTest()
        {
            //TODO: вынести генерацию OrderID из контроллера
            var msg = new OrderPlacingSagaRequest()
            {
                OrderID = Guid.NewGuid(),
                CustomerID = 111,
                Positions = new OrderPositionsMap() { { 1, 1 } },
                Price = new Money(200),
                PaymentProvider = EPaymentProvider.Mock
            };
            await _publishEndpoint.Publish(msg);

            //var response = await _createOrderClient.GetResponse<OrderPlacingSagaResponse>(
            //    new OrderPlacingSagaRequest()
            //    {
            //        OrderID = Guid.NewGuid(),
            //        CustomerID = 111,
            //        Positions = new OrderPositionsMap()
            //    });

            return Ok(msg);
        }

    }
}
