using FluentValidation;
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

    public class PaymentSetPaidByExternalIDMediatorResponse
    {
    }
    public class PaymentSetPaidByExternalIDMediatorRequest : IRequest<PaymentSetPaidByExternalIDMediatorResponse>
    {
        public string ExternalPaymentID { get; set; }
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
            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => e.ExternalPaymentID == request.ExternalPaymentID);
            payment.SetStatus(EPaymentStatus.Paid);
            return new PaymentSetPaidByExternalIDMediatorResponse();
        }
    }
}
