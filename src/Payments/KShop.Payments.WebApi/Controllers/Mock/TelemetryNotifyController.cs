using KShop.Communications.Contracts.Payments;
using KShop.Payments.Domain.Mediators;
using KShop.Payments.Domain.Providers.Mock.DTOs;
using KShop.Payments.Persistence.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.WebApi.Controllers.Mock
{

    [ApiController]
    [Route("[controller]")]
    public class TelemetryNotifyController : ControllerBase
    {
        private readonly ILogger<TelemetryNotifyController> _logger;

        public TelemetryNotifyController(
            ILogger<TelemetryNotifyController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Notify(string data)
        {
            _logger.LogWarning("Telemetry notify: {TelemetryNotify}", data);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ThrowException(string msg)
        {
            throw new Exception(msg);
            return Ok();
        }
    }
}
