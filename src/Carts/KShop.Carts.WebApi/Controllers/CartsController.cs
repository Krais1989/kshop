using KShop.Auth;
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
        public CartPosition()
        {
        }

        public CartPosition(uint productID, uint quantity)
        {
            ProductID = productID;
            Quantity = quantity;
        }

        public uint ProductID { get; set; }
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

        [HttpGet("my")]
        public async Task<IActionResult> Get()
        {
            var userId = this.GetCurrentUserID();
            var cart = await _cartService.GetAsync(userId.ToString());
            return Ok(cart);
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetPosition([FromBody]CartPosition position)
        {
            var userId = this.GetCurrentUserID();
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Add(position.ProductID, position.Quantity);
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }

        [HttpDelete("del/{productId}")]
        public async Task<IActionResult> RemovePosition(uint productId)
        {
            var userId = this.GetCurrentUserID();
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Remove(productId);
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var userId = this.GetCurrentUserID();
            string cartId = userId.ToString();
            var cart = await _cartService.GetAsync(userId.ToString());
            cart.Positions.Clear();
            await _cartService.UpdateAsync(cartId, cart);
            return Ok();
        }
    }
}
