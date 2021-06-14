using KShop.Carts.Persistence;
using KShop.Carts.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi.Controllers
{
    public class CartPosition
    {
        public ulong ProductID { get; set; }
        public uint Quantity { get; set; }
    }

    [ApiController]
    [Route("carts")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly ICartRepository _cartService;

        public CartsController(ILogger<CartsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ulong userId = 0;
            var cart = await _cartService.GetAsync(userId.ToString());
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> SetPosition([FromBody]CartPosition position)
        {
            ulong userId = 0;
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Add(position.ProductID, position.Quantity);
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemovePosition(ulong productId)
        {
            ulong userId = 0;
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Remove(productId);
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }

        public async Task<IActionResult> Clear()
        {
            ulong userId = 0;
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Clear();
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }
    }
}
