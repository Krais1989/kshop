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

        [HttpGet("[action]/{productId}")]
        public async ValueTask<IActionResult> TestProductReservePending(uint productId, uint quantity)
        {
            var entity = new ProductReserve()
            {
                OrderID = Guid.NewGuid(),
                ProductID = productId,
                Quantity = quantity,
                Status = ProductReserve.EStatus.Pending
            };
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpGet("home/{page:int}")]
        public async ValueTask<IActionResult> GetProductsForHomePage(int page)
        {
            var user = this.User;
            var request = new GetProductsForHomeMediatorRequest
            {
                PageIndex = page
            };

            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("details/{productId:int}")]
        public async ValueTask<IActionResult> GetProductDetails(uint productId)
        {
            var user = this.User;
            var request = new GetProductDetailsMediatorRequest
            {
                 ProductID = productId
            };

            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
