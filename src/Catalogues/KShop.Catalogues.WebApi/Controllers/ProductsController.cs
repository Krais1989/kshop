using KShop.Products.Persistence;
using MassTransit;
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

        public ProductsController(ILogger<ProductsController> logger, IPublishEndpoint pubEndpoint, ProductsContext context)
        {
            _logger = logger;
            _pubEndpoint = pubEndpoint;
            _context = context;
        }

        [HttpGet("{productId}")]
        public async ValueTask<IActionResult> Get(int productId)
        {
            var result = await _context.Products.Include(e => e.Positions).AsNoTracking().FirstOrDefaultAsync(e => e.ID == productId);
            return Ok(result);
        }
    }
}
