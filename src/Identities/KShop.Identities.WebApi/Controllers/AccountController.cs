using KShop.Identities.Domain;
using KShop.Shared.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Identities.WebApi
{
    public class ChangePasswordRequestDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }


    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        private IActionResult Return(BaseResponse response)
        {
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        public AccountController(ILogger<AccountController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register([FromBody] SignUpMediatorRequest dto)
        {
            var result = await _mediator.Send(dto);
            return Return(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            var request = new ChangePasswordMediatorRequest
            (
                user: this.User,
                oldPassword: dto.OldPassword,
                newPassword: dto.NewPassword
            );
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmByEmail(ConfirmByEmailMediatorRequest dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            var request = new DeleteAccountMediatorRequest(User);
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
