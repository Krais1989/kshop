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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.WebApi
{
    public class OrderCreateRequestDto
    {
        public string Address { get; set; }
        public EPaymentProvider PaymentProvider { get; set; }
        public EShippingMethod ShippingMethod { get; set; }
        public List<ProductQuantity> OrderContent { get; set; }
    }

    public class OrderCancelDto
    {
        public Guid OrderID { get; set; }
    }

    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly IMediator _mediator;

        public OrdersController(ILogger<OrdersController> logger,
                                IPublishEndpoint pubEndpoint, IMediator mediator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
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
        [HttpGet]
        public async ValueTask<IActionResult> GetCurrentCustomerOrders()
        {
            var customerId = this.GetCurrentUserIDExcept();
            var response = await _mediator.Send(new GetCustomerOrdersRequest { CustomerID = customerId });
            return ReturnBaseResponse(response);
        }

        [HttpPost]
        public async ValueTask<ActionResult> Create([FromBody] OrderCreateRequestDto dto)
        {
            var response = await _mediator.Send(new OrderPlacingMediatorRequest
            {
                UserID = this.GetCurrentUserIDExcept(),
                Address = dto.Address,
                OrderContent = dto.OrderContent,
                PaymentProvider = dto.PaymentProvider,
                ShippingMethod = dto.ShippingMethod
            });
            return Ok(response);
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
