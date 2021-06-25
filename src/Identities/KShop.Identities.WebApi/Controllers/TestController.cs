using KShop.Identities.Domain;
using KShop.Identities.Domain.SignIn.Mediators;
using KShop.Shared.Domain.Contracts;
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
    public class TestBadRequestException : BaseBadRequestException
    {
        public TestBadRequestException(string message = "This is test 400 exception") : base(message)
        {
        }
    }


    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMediator _mediator;

        public TestController(ILogger<TestController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("200")]
        public async Task<IActionResult> TestOk()
        {
            return Ok();
        }

        [HttpGet("400")]
        public async Task<IActionResult> TestBadRequest()
        {
            throw new TestBadRequestException("Test 400 exception");
        }

        [HttpGet("500")]
        public async Task<IActionResult> TestInternal()
        {
            throw new Exception("Test 500 exception");
        }

        [HttpGet("401")]
        [Authorize]
        public async Task<IActionResult> TestNotAuth()
        {
            return Ok("Current request is authorized, try not authorized for test");
        }
    }
}
