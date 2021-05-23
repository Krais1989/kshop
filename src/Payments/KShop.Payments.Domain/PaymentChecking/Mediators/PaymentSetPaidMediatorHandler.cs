﻿using FluentValidation;
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

    public class PaymentSetPaidMediatorResponse
    {
    }
    /// <summary>
    /// Установка статуса Оплачен для локально платежа
    /// </summary>
    public class PaymentSetPaidMediatorRequest : IRequest<PaymentSetPaidMediatorResponse>
    {
        public Guid PaymentID { get; set; }
    }
    public class PaymentSetPaidMediatorHandler : IRequestHandler<PaymentSetPaidMediatorRequest, PaymentSetPaidMediatorResponse>
    {
        private readonly ILogger<PaymentSetPaidMediatorHandler> _logger;
        private readonly PaymentsContext _paymentsContext;

        public PaymentSetPaidMediatorHandler(
            ILogger<PaymentSetPaidMediatorHandler> logger,
            PaymentsContext paymentsContext)
        {
            _logger = logger;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentSetPaidMediatorResponse> Handle(PaymentSetPaidMediatorRequest request, CancellationToken cancellationToken)
        {
            var payment = await _paymentsContext.Payments.Include(e => e.Logs).FirstOrDefaultAsync(e => e.ID == request.PaymentID);
            payment.SetStatus(EPaymentStatus.Paid);
            await _paymentsContext.SaveChangesAsync();
            return new PaymentSetPaidMediatorResponse();
        }
    }
}
