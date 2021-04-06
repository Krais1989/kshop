using KShop.Communications.Contracts.Orders;
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
        private readonly IRequestClient<OrderCreate_SagaRequest> _createOrderClient;

        public OrderTestController(IPublishEndpoint publishEndpoint, IRequestClient<OrderCreate_SagaRequest> createOrderClient)
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
            var response = await _createOrderClient.GetResponse<OrderCreate_SagaResponse>(
                new OrderCreate_SagaRequest()
                {
                    CustomerID = 111,
                    //Positions = new Dictionary<int,int>()
                });

            if (response.Message.IsSuccess)
            {
                return Ok(response.Message);
            }
            else
            {
                return Problem(response.Message.Message);
            }
        }
    }
}
