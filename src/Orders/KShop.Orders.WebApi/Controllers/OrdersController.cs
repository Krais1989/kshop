using KShop.Communications.Contracts.Orders;
using KShop.Communications.Contracts.Payments;
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
        private readonly IRequestClient<OrderGetStatusSagaRequest> _getOrderStatusClient;
        private readonly IRequestClient<OrderPlacingSagaRequest> _createOrderClient;

        public OrdersController(ILogger<OrdersController> logger,
                                IPublishEndpoint pubEndpoint,
                                OrderContext orderContext,
                                IRequestClient<OrderGetStatusSagaRequest> getOrderStatusClient,
                                IRequestClient<OrderPlacingSagaRequest> createOrderClient)
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
            var response = await _getOrderStatusClient.GetResponse<OrderGetStatusSagaResponse>(
                    new OrderGetStatusSagaRequest
                    {
                        OrderID = orderId
                    });


            if (response.Message.IsSuccess)
                return Ok(response.Message.Status);
            else
                return NotFound(response.Message);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Create([FromBody] OrderCreateRequestDto dto)
        {
            var orderCreateRequest = new OrderPlacingSagaRequest
            {
                OrderID = Guid.NewGuid(),
                PaymentProvider = EPaymentProvider.Mock,
                CustomerID = 1, // TODO: определять текущего юзера
                Positions = dto.Positions
            };

            await _pubEndpoint.Publish(orderCreateRequest);
            return Ok(orderCreateRequest);
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
