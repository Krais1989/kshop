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
        private readonly IRequestClient<CheckOrderSagaRequest> _checkOrderClient;
        private readonly IRequestClient<OrderCreateSagaRequest> _createOrderClient;


        public OrdersController(ILogger<OrdersController> logger, IPublishEndpoint pubEndpoint, OrderContext orderContext, IRequestClient<CheckOrderSagaRequest> checkOrderClient, IRequestClient<OrderCreateSagaRequest> createOrderClient)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _orderContext = orderContext;
            _checkOrderClient = checkOrderClient;
            _createOrderClient = createOrderClient;
        }


        [HttpGet("{orderId}")]
        public async ValueTask<IActionResult> Get(Guid orderId)
        {
            var (response, err) =
                await _checkOrderClient.GetResponse<CheckOrderSagaResponse, IOrderNotFoundResponse>(
                    new CheckOrderSagaRequest { OrderID = orderId });

            return response.IsCompletedSuccessfully ? Ok(await response) : NotFound(await err);

            //if (response.IsCompletedSuccessfully)
            //{
            //    return Ok(await response);
            //}
            //else
            //{
            //    return Ok(await err);
            //}

            ////var result = await _orderContext.Orders.Include(e => e.Positions).AsNoTracking().FirstOrDefaultAsync(e => e.ID == orderId);
            //return Ok(response);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Create([FromBody] OrderCreateRequestDto dto)
        {
            var orderCreateRequest = new OrderCreateSagaRequest
            {
                CustomerID = 1,
                Positions = dto.Positions.Select(e => new ProductStack { ProductID = e.ProductID, Quantity = e.Quantity }).ToList()
            };

            var response = await _createOrderClient.GetResponse<IOrderCreateSagaResponse>(orderCreateRequest);
            return Ok(response.Message);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Cancel([FromBody] OrderCancelDto dto)
        {
            /* Проверка данных для создания заказа */
            /* Создание заказа в БД */
            /* Инициализация саги создания заказа */

            var corId = Guid.NewGuid();
            await _pubEndpoint.Publish(new OrderCancelEvent { OrderID = dto.OrderID });
            return Ok();
        }
    }
}
