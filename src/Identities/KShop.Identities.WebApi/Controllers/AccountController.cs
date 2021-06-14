using KShop.Identities.Domain.SignIn.Mediators;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Identities.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        public AccountController(ILogger<AccountController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("current")]
        public async Task<IActionResult> Current()
        {
            var request = new AccountGetCurrentMediatorRequest { User = HttpContext.User };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountRegistrationMediatorRequest dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordMediatorRequest dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmByEmail(AccountEmailConfirmMediatorRequest dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            var request = new AccountDeleteMediatorRequest { };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
