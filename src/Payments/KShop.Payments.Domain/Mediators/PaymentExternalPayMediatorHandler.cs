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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Mediators
{

    public class PaymentExternalPayMediatorResponse
    {
    }
    public class PaymentExternalPayMediatorRequest : IRequest<PaymentExternalPayMediatorResponse>
    {
        /// <summary>
        /// ID платежа внешней системы
        /// </summary>
        public string ExternalPaymentID { get; set; }
        public EPaymentPlatformType PlatformType { get; set; }
    }
    public class PaymentExternalPayMediatorHandler : IRequestHandler<PaymentExternalPayMediatorRequest, PaymentExternalPayMediatorResponse>
    {
        private readonly ILogger<PaymentExternalPayMediatorHandler> _logger;
        private readonly IValidator<PaymentPayValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentExternalPayMediatorHandler(
            ILogger<PaymentExternalPayMediatorHandler> logger,
            IValidator<PaymentPayValidatorDto> validator,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentExternalPayMediatorResponse> Handle(PaymentExternalPayMediatorRequest request, CancellationToken cancellationToken)
        {
            /* Обработка ошибок */
            /* В БД уже имеются записи с ошибкой, которые из за эксепшенов снова и снова запрашиваются фоновыми обработчиками */

            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => string.Equals(e.ExternalPaymentID, request.ExternalPaymentID, StringComparison.InvariantCultureIgnoreCase));

            var validatorDto = new PaymentPayValidatorDto(
                externalPaymentID: request.ExternalPaymentID,
                platformType: request.PlatformType);
            var valid = _validator.Validate(validatorDto);
            if (!valid.IsValid)
            {
                /*  */
                payment.SetStatus(EPaymentStatus.Error, string.Join("|", valid.Errors.Select(e => e.ErrorMessage)));
            }
            else
            {
                payment.SetStatus(EPaymentStatus.Paid);
            }

            await _paymentsContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"\t\t>>> {payment.ID} {payment.Status} {payment.StatusDate} ");

            return new PaymentExternalPayMediatorResponse();
        }
    }
}
