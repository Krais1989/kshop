using FluentValidation;
using KShop.Payments.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{

    public class PaymentGetStatusMediatorResponse
    {
        public PaymentGetStatusMediatorResponse(Guid paymentID, EPaymentStatus status)
        {
            PaymentID = paymentID;
            Status = status;
        }

        public Guid PaymentID { get; private set; }
        public EPaymentStatus Status { get; private set; }
    }
    public class PaymentGetStatusMediatorRequest : IRequest<PaymentGetStatusMediatorResponse>
    {
        public PaymentGetStatusMediatorRequest(uint userID, Guid paymentID)
        {
            UserID = userID;
            PaymentID = paymentID;
        }

        public uint UserID { get; private set; }
        public Guid PaymentID { get; private set; }
    }
    public class PaymentGetStatusMediatorHandler : IRequestHandler<PaymentGetStatusMediatorRequest, PaymentGetStatusMediatorResponse>
    {
        private readonly ILogger<PaymentGetStatusMediatorHandler> _logger;
        private readonly IValidator<PaymentGetStatusFluentValidatorDto> _validator;

        private readonly PaymentsContext _context;

        public PaymentGetStatusMediatorHandler(ILogger<PaymentGetStatusMediatorHandler> logger, IValidator<PaymentGetStatusFluentValidatorDto> validator, PaymentsContext context)
        {
            _logger = logger;
            _validator = validator;
            _context = context;
        }

        public async Task<PaymentGetStatusMediatorResponse> Handle(PaymentGetStatusMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentGetStatusFluentValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = await _context.Payments.FirstOrDefaultAsync(e => e.ID == request.PaymentID);

            return new PaymentGetStatusMediatorResponse(
                paymentID: payment.ID,
                status: payment.Status                
            );
        }
    }
}
