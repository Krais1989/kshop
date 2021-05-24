using KShop.Shipments.Domain.Mediators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KShop.Shipments.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для внутреннего тестирования
    /// Содержит логику, которая в боевом режиме исполняется консумерами через шину
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestShipmentsController : ControllerBase
    {
        private readonly ILogger<ExternalShipmentsController> _logger;
        private readonly IMediator _mediator;

        public TestShipmentsController(ILogger<ExternalShipmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateShipment([FromBody] ShipmentInitializeMediatorRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CancelShipment([FromBody] ShipmentCancelMediatorRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var response = await _mediator.Send(new ShipmentGetByIdRequest { ShipmentID = id });
            return Ok(response);
        }
    }
}
