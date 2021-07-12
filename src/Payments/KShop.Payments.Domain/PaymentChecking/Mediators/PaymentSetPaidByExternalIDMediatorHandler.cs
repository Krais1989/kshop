using FluentValidation;
using KShop.Payments.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{

    public class PaymentSetPaidByExternalIDMediatorResponse
    {
    }
    public class PaymentSetPaidByExternalIDMediatorRequest : IRequest<PaymentSetPaidByExternalIDMediatorResponse>
    {
        public PaymentSetPaidByExternalIDMediatorRequest(uint userID, string externalPaymentID)
        {
            UserID = userID;
            ExternalPaymentID = externalPaymentID;
        }

        public uint UserID { get; private set; }
        public string ExternalPaymentID { get; private set; }
    }
    public class PaymentSetPaidByExternalIDMediatorHandler : IRequestHandler<PaymentSetPaidByExternalIDMediatorRequest, PaymentSetPaidByExternalIDMediatorResponse>
    {
        private readonly ILogger<PaymentSetPaidByExternalIDMediatorHandler> _logger;
        private readonly PaymentsContext _paymentsContext;

        public PaymentSetPaidByExternalIDMediatorHandler(
            ILogger<PaymentSetPaidByExternalIDMediatorHandler> logger,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentSetPaidByExternalIDMediatorResponse> Handle(PaymentSetPaidByExternalIDMediatorRequest request, CancellationToken cancellationToken)
        {
            var payment = await _paymentsContext.Payments.Include(e => e.Logs)
                .FirstOrDefaultAsync(e => e.ExternalID == request.ExternalPaymentID && e.UserID == request.UserID);
            payment.SetStatus(EPaymentStatus.Paid);
            return new PaymentSetPaidByExternalIDMediatorResponse();
        }
    }
}
