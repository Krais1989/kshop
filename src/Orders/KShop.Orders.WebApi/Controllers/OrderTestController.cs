using KShop.Orders.Domain.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRequestClient<CreateOrder_RoutingSlipRequest> _createOrderClient;

        public OrderTestController(IPublishEndpoint publishEndpoint, IRequestClient<CreateOrder_RoutingSlipRequest> createOrderClient)
        {
            _publishEndpoint = publishEndpoint;
            _createOrderClient = createOrderClient;
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
            var response = await _createOrderClient.GetResponse<CreateOrderSuccess_RoutingSlipMessage, CreateOrderFailure_RoutingSlipMessage>(
                new CreateOrder_RoutingSlipRequest()
                {

                });

            if (response.Item1 != null)
                return Ok((await response.Item1).Message);
            if (response.Item2 != null)
                return Problem((await response.Item2).Message.ErrorMessage);
            return Ok("ERROR Response is empty! ");
        }
    }
}
