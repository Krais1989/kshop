using KShop.Communications.Contracts.Orders;
using KShop.Orders.Domain.ValueObjects;
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

        public OrdersController(ILogger<OrdersController> logger, IPublishEndpoint pubEndpoint, OrderContext orderContext, IRequestClient<CheckOrderSagaRequest> checkOrderClient)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _orderContext = orderContext;
            _checkOrderClient = checkOrderClient;
        }


        [HttpGet("{orderId}")]
        public async ValueTask<IActionResult> Get(Guid orderId)
        {
            var (response, err) = 
                await _checkOrderClient.GetResponse<CheckOrderSagaResponse,IOrderNotFoundResponse>(
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
        public async ValueTask<ActionResult> Create([FromBody] OrderCreateDto dto)
        {
            /* Проверка данных для создания заказа */
            /* Создание заказа в БД */
            var entity = new Order()
            {
                Status = Order.EStatus.Initial,
                Positions = dto.Positions.Select(e => new OrderPosition() { ProductID = e.ProductID, Quantity = e.Quantity }).ToList()
            };
            await _orderContext.AddAsync(entity);
            await _orderContext.SaveChangesAsync();

            /* Событие инициации саги */
            var orderCreateEvent = new OrderCreateSagaRequest
            {
                OrderID = entity.ID,
                Positions = dto.Positions.Select(e => new ProductStack { ProductID = e.ProductID, Quantity = e.Quantity }).ToList()
            };

            await _pubEndpoint.Publish(orderCreateEvent);
            return Ok(entity.ID);
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
