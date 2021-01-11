﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.WebApi.Controllers
{
    public class PaymentStatusDto
    {
    }

    public class PaymentModyfi
    {

    }

    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получить статус платежа
        /// </summary>
        [HttpGet("[action]")]
        public async Task<ActionResult> GetStatus(Guid orderId)
        {
            return Ok();
        }

        /// <summary>
        /// Redirect-url для платежной системы, принимает запросы с результатами платежа
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult> SetPayment([FromQuery] PaymentModyfi paymentInfo)
        {
            return Ok();
        }
    }
}