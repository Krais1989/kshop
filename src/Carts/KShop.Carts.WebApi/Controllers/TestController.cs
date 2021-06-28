using KShop.Carts.Persistence;
using KShop.Shared.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ICartKVRepository _cartRepo;

        public TestController(ILogger<TestController> logger, ICartKVRepository cartRepo)
        {
            _logger = logger;
            _cartRepo = cartRepo;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            var result = await _cartRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create()
        {
            var cart = new Cart
            {
                ID = "test_cart",
                Positions = new List<CartPosition> {
                    new CartPosition(1, 1, false, "Product #1", new Money(100), "", "" ),
                    new CartPosition(2, 1, false, "Product #2", new Money(100), "", "" ),
                    new CartPosition(3, 1, false, "Product #3", new Money(100), "", "" ),
                }
            };
            var result = await _cartRepo.InsertAsync(cart);

            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] string id)
        {
            var cart = await _cartRepo.GetAsync(id);
            var nextId = cart.Positions.Last().ProductID + 1;
            cart.Positions.Add(
                new CartPosition(nextId,
                1,
                false,
                $"Product {nextId}",
                new Money(100),
                $"Description for product {nextId}",
                ""));
            await _cartRepo.ReplaceAsync(id, cart);
            return Ok(cart);
        }


    }
}
