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

    public class PaymentSetCanceledByExternalIDMediatorResponse
    {
    }
    public class PaymentSetCanceledByExternalIDMediatorRequest : IRequest<PaymentSetCanceledByExternalIDMediatorResponse>
    {
        public string ExternalPaymentID { get; set; }
    }
    public class PaymentSetCanceledByExternalIDMediatorHandler : IRequestHandler<PaymentSetCanceledByExternalIDMediatorRequest, PaymentSetCanceledByExternalIDMediatorResponse>
    {
        private readonly ILogger<PaymentSetCanceledByExternalIDMediatorHandler> _logger;
        private readonly PaymentsContext _paymentsContext;

        public PaymentSetCanceledByExternalIDMediatorHandler(
            ILogger<PaymentSetCanceledByExternalIDMediatorHandler> logger,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentSetCanceledByExternalIDMediatorResponse> Handle(PaymentSetCanceledByExternalIDMediatorRequest request, CancellationToken cancellationToken)
        {
            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => e.ExternalID == request.ExternalPaymentID);
            payment.SetStatus(EPaymentStatus.Canceled);
            await _paymentsContext.SaveChangesAsync();
            return new PaymentSetCanceledByExternalIDMediatorResponse();
        }
    }
}
