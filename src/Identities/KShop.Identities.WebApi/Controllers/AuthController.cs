using KShop.Identities.Domain;
using KShop.Identities.Domain.SignIn.Mediators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Identities.WebApi
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("current")]
        public async Task<IActionResult> Current()
        {
            var request = new GetCurrentIdentityMediatorRequest { User = this.User };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInByEmailPasswordMediatorHandlerRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
