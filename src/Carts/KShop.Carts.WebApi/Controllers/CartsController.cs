using KShop.Carts.Domain.Mediators;
using KShop.Carts.Persistence;
using KShop.Shared.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Carts.WebApi
{
    public class SetCartPositionsRequestDto
    {
        public List<CartPosition> positions { get; set; }
    }

    public class RemoveCartPositionsRequestDto
    {
        public List<uint> productIds { get; set; }
    }


    [ApiController]
    [Route("carts")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly IMediator _mediator;

        public CartsController(ILogger<CartsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("current")]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetCurrentCartMediatorRequest()
            {
                UserID = this.GetCurrentUserIDExcept()
            });
            return Ok(response);
        }

        [HttpPost("set-positions")]
        public async Task<IActionResult> SetPosition([FromBody] SetCartPositionsRequestDto dto)
        {
            var response = await _mediator.Send(new SetCartPositionsMediatorRequest()
            {
                UserID = this.GetCurrentUserIDExcept(),
                Positions = dto.positions
            });
            return Ok(response);
        }

        [HttpDelete("remove-positions")]
        public async Task<IActionResult> RemovePositions([FromBody] RemoveCartPositionsRequestDto dto)
        {
            var response = await _mediator.Send(new RemoveCartPositionMediatorRequest
            {
                UserID = this.GetCurrentUserIDExcept(),
                ProductIDs = dto.productIds
            });
            return Ok(response);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var response = await _mediator.Send(new ClearCartMediatorRequest
            {
                UserID = this.GetCurrentUserIDExcept()
            });
            return Ok(response);
        }
    }
}
