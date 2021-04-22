using FluentValidation;
using KShop.Communications.Contracts.Payments;
using KShop.Payments.Domain.Validators;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Mediators
{
    public class PaymentExternalCancelMediatorResponse
    {
    }

    /// <remarks>
    /// Отмена платежа вызывается при ответе от внешней платежной системы
    /// Поскольку, внешние платежные системы ничего не знают про ID внутреннего платежа, в классе используется ExternalPaymentID
    /// </remarks>
    public class PaymentExternalCancelMediatorRequest : IRequest<PaymentExternalCancelMediatorResponse>
    {
        /// <summary>
        /// ID платежа внешней системы
        /// </summary>
        public string ExternalPaymentID { get; set; }
        public EPaymentPlatformType PaymentPlatformType { get; set; }
    }
    public class PaymentExternalCancelMediatorHandler : IRequestHandler<PaymentExternalCancelMediatorRequest, PaymentExternalCancelMediatorResponse>
    {
        private readonly ILogger<PaymentExternalCancelMediatorHandler> _logger;
        private readonly IValidator<PaymentCancelExternalValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentExternalCancelMediatorHandler(
            ILogger<PaymentExternalCancelMediatorHandler> logger,
            IValidator<PaymentCancelExternalValidatorDto> validator,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentExternalCancelMediatorResponse> Handle(PaymentExternalCancelMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentCancelExternalValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = await _paymentsContext.Payments.Include(e => e.Logs)
                .FirstOrDefaultAsync(e =>
                    e.PaymentPlatformType == request.PaymentPlatformType
                    && e.ExternalPaymentID == request.ExternalPaymentID);

            payment.SetStatus(EPaymentStatus.Canceled);
            await _paymentsContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"\t\t>>> {payment.ID} {payment.Status} {payment.StatusDate} ");

            return new PaymentExternalCancelMediatorResponse();
        }
    }
}
