using KShop.Communications.Contracts.Products;
using KShop.Products.Domain.ProductsReservation.Mediators;
using KShop.Products.Persistence;
using KShop.Products.Persistence.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async ValueTask<IActionResult> Get(ulong productId)
        {
            var result = await _context.Products.Include(e => e.Positions).AsNoTracking().FirstOrDefaultAsync(e => e.ID == productId);
            return Ok(result);
        }


        [HttpGet("[action]/{productId}")]
        public async ValueTask<IActionResult> TestProductReservePending(ulong productId, uint quantity)
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
    }
}
