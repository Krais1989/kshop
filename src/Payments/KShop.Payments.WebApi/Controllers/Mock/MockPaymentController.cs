using KShop.Shared.Domain.Contracts;

using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KShop.Shared.Integration.Contracts;
using KShop.Payments.Domain;
using KShop.Payments.Persistence;
using KShop.Shared.Authentication;

namespace KShop.Payments.WebApi
{
    public class MockPaymentCreateRequestApiDto
    {
        public MockPaymentCreateRequestApiDto(uint userID, Guid orderID, Money price)
        {
            UserID = userID;
            OrderID = orderID;
            Price = price;
        }

        public uint UserID { get; private set; }
        public Guid OrderID { get; private set; }
        public Money Price { get; private set; }
    }
    public class MockPaymentCreateResponseApiDto
    {
        public MockPaymentCreateResponseApiDto(Guid paymentID, string externalPaymentID)
        {
            PaymentID = paymentID;
            ExternalPaymentID = externalPaymentID;
        }

        public Guid PaymentID { get; private set; }
        public string ExternalPaymentID { get; private set; }
    }

    public class MockPaymentStatusRequestApiDto
    {
        public MockPaymentStatusRequestApiDto(uint userID, Guid paymentID)
        {
            UserID = userID;
            PaymentID = paymentID;
        }

        public uint UserID { get; private set; }
        public Guid PaymentID { get; private set; }
    }

    public class MockPaymentStatusResponseApiDto
    {
        public MockPaymentStatusResponseApiDto(EPaymentStatus status)
        {
            Status = status;
        }

        public EPaymentStatus Status { get; private set; }
    }

    public class MockCancelPaymentRequestApiDto
    {
        public MockCancelPaymentRequestApiDto(uint userID, Guid paymentID)
        {
            UserID = userID;
            PaymentID = paymentID;
        }

        public uint UserID { get; private set; }
        public Guid PaymentID { get; private set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class MockPaymentController : ControllerBase
    {
        private readonly ILogger<MockPaymentController> _logger;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<PaymentCreateSvcRequest> _createClient;
        private readonly IRequestClient<PaymentCancelSvcRequest> _cancelClient;

        public MockPaymentController(
            ILogger<MockPaymentController> logger,
            IMediator mediator,
            IPublishEndpoint publishEndpoint,
            IRequestClient<PaymentCreateSvcRequest> createClient,
            IRequestClient<PaymentCancelSvcRequest> cancelClient)
        {
            _logger = logger;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _createClient = createClient;
            _cancelClient = cancelClient;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ThrowException(string msg)
        {
            throw new Exception(msg);
            return Ok();
        }

        /* Тестовая логика, Payment создаётся только через консумер, тк является частью саги */
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePaymentTest([FromBody] MockPaymentCreateRequestApiDto dto)
        {
            var createBusReq = new PaymentCreateSvcRequest(
                orderID: dto.OrderID,
                userID: dto.UserID,
                paymentPlatform: EPaymentProvider.Mock,
                money: dto.Price);

            var result = await _createClient.GetResponse<PaymentCreateSuccessSvcEvent>(createBusReq);

            return Ok(result.Message);
        }

        /// <summary>
        /// Получить статус платежа
        /// </summary>
        [HttpGet("[action]")]
        public async Task<ActionResult> GetStatus([FromQuery] MockPaymentStatusRequestApiDto dto)
        {
            var statusReq = new PaymentGetStatusMediatorRequest
            (
                paymentID: dto.PaymentID,
                userID: this.GetCurrentUserID().Value
            );
            var statusResp = await _mediator.Send(statusReq);
            return Ok(statusResp);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CancelPayment([FromBody] MockCancelPaymentRequestApiDto dto)
        {
            var cancelReq = new PaymentCancelSvcRequest(dto.PaymentID, dto.UserID);
            var cancelResp = await _cancelClient.GetResponse<PaymentCancelSvcResponse>(cancelReq);
            return Ok(cancelResp);
        }

        /// <summary>
        /// Redirect-url для платежной системы, принимает запросы с результатами платежа
        /// </summary>
        [HttpGet("[action]")]
        public async Task<ActionResult> PaymentCallback([FromQuery] MockPaymentCallbackDto dto)
        {
            switch (dto.Status)
            {
                case EMockPaymentExternalStatus.Paid:
                    var payReq = new PaymentSetPaidByExternalIDMediatorRequest
                    (
                        externalPaymentID: dto.ExternalPaymentID,
                        userID: this.GetCurrentUserID().Value
                    );
                    var payResp = await _mediator.Send(payReq);
                    break;
                case EMockPaymentExternalStatus.Canceled:
                    var cancelReq = new PaymentSetCanceledByExternalIDMediatorRequest
                    (
                        externalPaymentID: dto.ExternalPaymentID,
                        userID: this.GetCurrentUserID().Value
                    );
                    var cancelResp = await _mediator.Send(cancelReq);
                    break;
                case EMockPaymentExternalStatus.Error:
                    break;
            }

            return Ok();
        }
    }
}
