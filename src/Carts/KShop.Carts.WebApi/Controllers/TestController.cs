using KShop.Carts.Persistence;
using KShop.Carts.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ICartRepository _cartRepo;

        public TestController(ILogger<TestController> logger, ICartRepository cartRepo)
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
                Positions = new CartPositions { { 1, 1}, { 2, 1 } }
            };
            var result = await _cartRepo.CreateAsync(cart);

            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody]string id)
        {
            var cart = await _cartRepo.GetAsync(id);
            cart.Positions.Add(cart.Positions.Last().Key + 1, 1);
            await _cartRepo.UpdateAsync(id, cart);
            return Ok(cart);
        }


    }
}
