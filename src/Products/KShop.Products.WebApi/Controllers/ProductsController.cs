using KShop.Shared.Domain.Contracts;
using KShop.Products.Persistence;

using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KShop.Products.Domain.Mediators;

namespace KShop.Products.WebApi
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IPublishEndpoint _pubEndpoint;
        private readonly ProductsContext _context;
        private readonly IMediator _mediator;

        public ProductsController(
            ILogger<ProductsController> logger,
            IPublishEndpoint pubEndpoint,
            ProductsContext context,
            IMediator mediator)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet("for-home")]
        public async ValueTask<IActionResult> GetProductsForHomePage([FromQuery] GetProductsForHomeMediatorRequest request)
        {
            var user = this.User;
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("details")]
        public async ValueTask<IActionResult> GetProductsDetails([FromQuery] GetProductDetailsMediatorRequest request)
        {
            //var user = this.User;
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("for-order")]
        public async ValueTask<IActionResult> GetProductForOrder([FromQuery] GetProductsForOrderMediatorRequest request)
        {
            var user = this.User;
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
