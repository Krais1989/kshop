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
using KShop.Products.Domain;
using KShop.Shared.Authentication;

namespace KShop.Products.WebApi
{
    public class ProductsGetForHomeRequest
    {
        public int PageIndex { get; set; }
    }

    public class ProductGetDetailsRequest
    {
        public uint[] Data { get; set; }
    }

    public class ProductsGetForOrderRequest
    {
        public uint[] ProductsIDs { get; set; }
    }

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

        [HttpGet("bookmarked")]
        public async ValueTask<IActionResult> GetProductsBookmarked()
        {
            var response = await _mediator.Send(new ProductsGetBookmarkedMediatorRequest
            (
                userID: this.GetCurrentUserID().Value
            ));

            return Ok(response);
        }

        [HttpGet("for-home")]
        public async ValueTask<IActionResult> GetProductsForHomePage([FromQuery] ProductsGetForHomeMediatorRequest dto)
        {
            var request = new ProductsGetForHomeMediatorRequest(dto.PageIndex, this.GetCurrentUserID());
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("details")]
        public async ValueTask<IActionResult> GetProductsDetails([FromQuery] ProductGetDetailsRequest dto)
        {
            var request = new ProductGetDetailsMediatorRequest(productID: dto.Data, userID: this.GetCurrentUserID());
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("for-order")]
        public async ValueTask<IActionResult> GetProductForOrder([FromQuery] ProductsGetForOrderRequest dto)
        {
            var request = new ProductsGetForOrderMediatorRequest(productsIDs: dto.ProductsIDs, userID: this.GetCurrentUserID());
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
