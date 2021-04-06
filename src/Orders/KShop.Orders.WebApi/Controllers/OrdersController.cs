using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.ValueObjects;
using KShop.Orders.Persistence;
using KShop.Orders.Persistence.Entities;
using KShop.Orders.WebApi.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _orderContext;
        private readonly IRequestClient<OrderGetStatus_SagaRequest> _getOrderStatusClient;
        private readonly IRequestClient<OrderCreate_SagaRequest> _createOrderClient;

        public OrdersController(ILogger<OrdersController> logger,
                                IPublishEndpoint pubEndpoint,
                                OrderContext orderContext,
                                IRequestClient<OrderGetStatus_SagaRequest> getOrderStatusClient,
                                IRequestClient<OrderCreate_SagaRequest> createOrderClient)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _orderContext = orderContext;
            _getOrderStatusClient = getOrderStatusClient;
            _createOrderClient = createOrderClient;
        }

        [HttpGet("{orderId}")]
        public async ValueTask<IActionResult> Get(Guid orderId)
        {
            var (response, err) =
                await _getOrderStatusClient.GetResponse<OrderGetStatus_SagaRequest, OrderGetStatus_SagaResponse>(
                    new OrderGetStatus_SagaRequest { OrderID = orderId });

            if (response.IsCompletedSuccessfully)
            {
                return Ok(await response);
            }
            else
            {
                return NotFound(await err);
            }
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Create([FromBody] OrderCreateRequestDto dto)
        {
            var orderCreateRequest = new OrderCreate_SagaRequest
            {
                CustomerID = 1,
                Positions = dto.Positions
            };

            var response = await _createOrderClient.GetResponse<OrderCreate_SagaRequest>(orderCreateRequest);
            return Ok(response.Message);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Cancel([FromBody] OrderCancelDto dto)
        {
            /* Проверка данных для создания заказа */
            /* Создание заказа в БД */
            /* Инициализация саги создания заказа */

            return Ok();
        }
    }
}
