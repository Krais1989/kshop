using FluentValidation;
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

    public class PaymentApproveMediatorResponse
    {
    }
    public class PaymentApproveMediatorRequest : IRequest<PaymentApproveMediatorResponse>
    {
        public Guid PaymentID { get; set; }
    }
    public class PaymentApproveMediatorHandler : IRequestHandler<PaymentApproveMediatorRequest, PaymentApproveMediatorResponse>
    {
        private readonly ILogger<PaymentApproveMediatorHandler> _logger;
        private readonly IValidator<PaymentApproveValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentApproveMediatorHandler(
            ILogger<PaymentApproveMediatorHandler> logger, 
            IValidator<PaymentApproveValidatorDto> validator, 
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentApproveMediatorResponse> Handle(PaymentApproveMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentApproveValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => e.ID == request.PaymentID);

            payment.Status = Payment.EStatus.Success;
            payment.StatusDate = DateTime.UtcNow;
            payment.Logs.Add(new PaymentLog() { ModifyDate = DateTime.UtcNow, Status = Payment.EStatus.Success });
            await _paymentsContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"\t\t>>> {payment.ID} {payment.StatusDate} ");

            return new PaymentApproveMediatorResponse();
        }
    }
}
