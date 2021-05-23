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
    /// <summary>
    /// Установка статуса Оплачен для локально платежа
    /// </summary>
    public class PaymentSetCanceledMediatorRequest : IRequest<PaymentCancelMediatorResponse>
    {
        public Guid PaymentID { get; set; }
    }
    public class PaymentSetCanceledMediatorHandler : IRequestHandler<PaymentSetCanceledMediatorRequest, PaymentCancelMediatorResponse>
    {
        private readonly ILogger<PaymentSetCanceledMediatorHandler> _logger;
        private readonly PaymentsContext _paymentsContext;

        public PaymentSetCanceledMediatorHandler(
            ILogger<PaymentSetCanceledMediatorHandler> logger,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentCancelMediatorResponse> Handle(PaymentSetCanceledMediatorRequest request, CancellationToken cancellationToken)
        {
            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => e.ID == request.PaymentID);
            payment.SetStatus(EPaymentStatus.Canceled);
            await _paymentsContext.SaveChangesAsync(cancellationToken);
            return new PaymentCancelMediatorResponse();
        }
    }
}
