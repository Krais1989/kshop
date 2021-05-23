using FluentValidation;
using KShop.Communications.Contracts.Payments;
using KShop.Payments.Domain.Validators;
using KShop.Payments.Persistence;
using KShop.Payments.Persistence.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain.Mediators
{
    public class PaymentInitializeMediatorResponse
    {
        public Guid PaymentID { get; set; }
    }
    public class PaymentInitializeMediatorRequest : IRequest<PaymentInitializeMediatorResponse>
    {
        public EPaymentProvider PaymentPlatform { get; set; }
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }
    public class PaymentInitializeMediatorHandler : IRequestHandler<PaymentInitializeMediatorRequest, PaymentInitializeMediatorResponse>
    {
        private readonly ILogger<PaymentInitializeMediatorHandler> _logger;
        private readonly IValidator<PaymentCreatedValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentInitializeMediatorHandler(ILogger<PaymentInitializeMediatorHandler> logger, IValidator<PaymentCreatedValidatorDto> validator, PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentInitializeMediatorResponse> Handle(PaymentInitializeMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentCreatedValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = new Payment()
            {
                OrderID = request.OrderID,
                PaymentProvider = request.PaymentPlatform
            };

            payment.SetStatus(EPaymentStatus.Initializing);

            await _paymentsContext.AddAsync(payment);
            await _paymentsContext.SaveChangesAsync();

            return new PaymentInitializeMediatorResponse()
            {
                PaymentID = payment.ID,
            };
        }
    }
}