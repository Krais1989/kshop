using KShop.Orders.Domain;
using KShop.Orders.Persistence;
using KShop.Shared.Authentication;
using KShop.Shared.Domain.Contracts;
using KShop.Shared.Integration.Contracts;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.WebApi
{

    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly OrderContext _orderContext;
        private readonly IRequestClient<OrderGetStatusSagaRequest> _getOrderStatusClient;
        private readonly IRequestClient<OrderPlacingSagaRequest> _createOrderClient;
        private readonly IMediator _mediator;

        public OrdersController(ILogger<OrdersController> logger,
                                IPublishEndpoint pubEndpoint,
                                OrderContext orderContext,
                                IRequestClient<OrderGetStatusSagaRequest> getOrderStatusClient,
                                IRequestClient<OrderPlacingSagaRequest> createOrderClient, IMediator mediator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _orderContext = orderContext;
            _getOrderStatusClient = getOrderStatusClient;
            _createOrderClient = createOrderClient;
            _mediator = mediator;
        }

        private IActionResult ReturnBaseResponse(BaseResponse response)
        {
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// Статус заказа 
        /// Данные берутся из состояния соответствующей Саги
        /// </summary>
        [HttpGet("status/{orderId}")]
        public async ValueTask<IActionResult> GetStatus(Guid orderId)
        {
            var customerId = this.GetCurrentUserIDExcept();
            var response = await _mediator.Send(new OrderGetDetailsRequest { CustomerID = customerId, OrderID = orderId });
            return ReturnBaseResponse(response);
        }

        /// <summary>
        /// Детали заказа
        /// </summary>
        [HttpGet("details/{orderId}")]
        public async ValueTask<IActionResult> GetDetails(Guid orderId)
        {
            var customerId = this.GetCurrentUserIDExcept();
            var response = await _mediator.Send(new OrderGetDetailsRequest { CustomerID = customerId, OrderID = orderId });
            return ReturnBaseResponse(response);
        }

        /// <summary>
        /// Заказа текущего пользователя
        /// </summary>
        [HttpGet("all")]
        public async ValueTask<IActionResult> GetAll()
        {
            var customerId = this.GetCurrentUserIDExcept();
            var response = await _mediator.Send(new OrderGetAllRequest { CustomerID = customerId });
            return ReturnBaseResponse(response);
        }

        [HttpPost]
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

        [HttpPost("cancel")]
        public async ValueTask<ActionResult> Cancel([FromBody] OrderCancelDto dto)
        {
            /* Проверка данных для создания заказа */
            /* Создание заказа в БД */
            /* Инициализация саги создания заказа */

            // TODO: логика отмены заказа

            return Ok();
        }
    }
}
