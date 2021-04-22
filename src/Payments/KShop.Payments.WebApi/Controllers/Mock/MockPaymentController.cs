using KShop.Communications.Contracts.Invoices;
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
    public class MockPaymentCreateRequestApiDto
    {
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }
    public class MockPaymentCreateResponseApiDto
    {
        public Guid PaymentID { get; set; }
        public string ExternalPaymentID { get; set; }
    }

    public class MockPaymentStatusRequestApiDto
    {
        public Guid PaymentID { get; set; }
    }

    public class MockPaymentStatusResponseApiDto
    {
        public EPaymentStatus Status { get; set; }
    }

    public class MockCancelPaymentRequestApiDto
    {
        public Guid PaymentID { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class MockPaymentController : ControllerBase
    {
        private readonly ILogger<MockPaymentController> _logger;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public MockPaymentController(
            ILogger<MockPaymentController> logger,
            IMediator mediator,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        /* Тестовая логика, Payment создаётся только через консумер, тк является частью саги */
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePaymentTest([FromBody] MockPaymentCreateRequestApiDto dto)
        {
            var paymentId = Guid.NewGuid();

            var createBusReq = new PaymentCreateBusRequest()
            {
                CorrelationID = paymentId,
                PaymentPlatform = EPaymentPlatformType.Mock,
                OrderID = dto.OrderID,
                Price = dto.Price,
            };
            await _publishEndpoint.Publish(createBusReq);
            return Ok();
        }

        /// <summary>
        /// Получить статус платежа
        /// </summary>
        [HttpGet("[action]")]
        public async Task<ActionResult> GetStatus([FromQuery] MockPaymentStatusRequestApiDto dto)
        {
            var statusReq = new PaymentGetStatusMediatorRequest()
            {
                PaymentID = dto.PaymentID
            };
            var statusResp = await _mediator.Send(statusReq);

            return Ok(statusResp);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CancelPayment([FromBody] MockCancelPaymentRequestApiDto dto)
        {
            var cancelReq = new PaymentCancelBusRequest() {
                CorrelationID = dto.PaymentID
            };
            await _publishEndpoint.Publish(cancelReq);
            return Ok();
        }

        /// <summary>
        /// Redirect-url для платежной системы, принимает запросы с результатами платежа
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ActionResult> PaymentCallback([FromQuery] MockPaymentCallbackDto dto)
        {
            switch (dto.Status)
            {
                case EMockPaymentExternalStatus.Paid:
                    var payReq = new PaymentExternalPayMediatorRequest()
                    {
                        ExternalPaymentID = dto.ExternalPaymentID,
                        PlatformType = EPaymentPlatformType.Mock
                    };
                    var payResp = await _mediator.Send(payReq);
                    break;
                case EMockPaymentExternalStatus.Canceled:
                    var cancelReq = new PaymentExternalCancelMediatorRequest()
                    {
                        ExternalPaymentID = dto.ExternalPaymentID,
                        PaymentPlatformType = EPaymentPlatformType.Mock
                    };
                    var cancelResp = await _mediator.Send(cancelReq);
                    break;
                case EMockPaymentExternalStatus.Error:
                    break;
            }

            return Ok();
        }
    }
}
