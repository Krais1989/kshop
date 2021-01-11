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

    public class PaymentRejectResponse
    {
    }
    public class PaymentRejectRequest : IRequest<PaymentRejectResponse>
    {
        public Guid PaymentID { get; set; }
    }
    public class PaymentRejectMediatorHandler : IRequestHandler<PaymentRejectRequest, PaymentRejectResponse>
    {
        private readonly ILogger<PaymentRejectMediatorHandler> _logger;
        private readonly IValidator<PaymentRejectValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentRejectMediatorHandler(
            ILogger<PaymentRejectMediatorHandler> logger, 
            IValidator<PaymentRejectValidatorDto> validator, 
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentRejectResponse> Handle(PaymentRejectRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentRejectValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = await _paymentsContext.Payments.Include(e=>e.Logs).FirstOrDefaultAsync(e => e.ID == request.PaymentID);

            payment.Status = Payment.EStatus.Rejected;
            payment.StatusDate = DateTime.UtcNow;
            payment.Logs.Add(new PaymentLog() { ModifyDate = DateTime.UtcNow, Status = Payment.EStatus.Rejected });
            await _paymentsContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"\t\t>>> {payment.ID} {payment.StatusDate} ");

            return new PaymentRejectResponse();
        }
    }
}
