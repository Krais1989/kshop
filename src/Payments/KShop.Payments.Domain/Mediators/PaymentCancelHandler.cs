using FluentValidation;
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

    public class PaymentCancelMediatorResponse
    {
    }

    public class PaymentCancelMediatorRequest : IRequest<PaymentCancelMediatorResponse>
    {
        /// <summary>
        /// Внутренний ID платежа
        /// </summary>
        public Guid PaymentID { get; set; }
    }
    public class PaymentCancelHandler : IRequestHandler<PaymentCancelMediatorRequest, PaymentCancelMediatorResponse>
    {
        private readonly ILogger<PaymentCancelHandler> _logger;
        private readonly IValidator<PaymentCancelInternalFluentValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentCancelHandler(
            ILogger<PaymentCancelHandler> logger,
            IValidator<PaymentCancelInternalFluentValidatorDto> validator,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentCancelMediatorResponse> Handle(PaymentCancelMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentCancelInternalFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = await _paymentsContext.Payments.Include(e => e.Logs)
                .FirstOrDefaultAsync(e => e.ID == request.PaymentID);

            payment.SetStatus(EPaymentStatus.Canceled);
            await _paymentsContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"\t\t>>> {payment.ID} {payment.Status} {payment.StatusDate} ");

            return new PaymentCancelMediatorResponse();
        }
    }
}
