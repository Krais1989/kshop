using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Shipments.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalShipmentsController : ControllerBase
    {
        private readonly ILogger<ExternalShipmentsController> _logger;

        /// <summary>
        /// Метод вызываемый внешней системой при изменениях статуса доставки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task <IActionResult> ExternalServiceCallback()
        {
            return Ok();
        }
    }
}
