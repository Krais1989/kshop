using FluentValidation;
using KShop.Shared.Domain.Contracts;
using KShop.Payments.Persistence;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Payments.Domain
{
    public class PaymentInitializeMediatorResponse : BaseResponse
    {
        public PaymentInitializeMediatorResponse(Guid paymentID)
        {
            PaymentID = paymentID;
        }

        public PaymentInitializeMediatorResponse(string error) : base(error)
        {
        }

        public Guid PaymentID { get; private set; }
    }
    public class PaymentInitializeMediatorRequest : IRequest<PaymentInitializeMediatorResponse>
    {
        public PaymentInitializeMediatorRequest(EPaymentProvider paymentPlatform, Guid orderID, Money money)
        {
            PaymentPlatform = paymentPlatform;
            OrderID = orderID;
            Money = money;
        }

        public EPaymentProvider PaymentPlatform { get; private set; }
        public Guid OrderID { get; private set; }
        public Money Money { get; private set; }
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
            var validator_result = _validator.Validate(validatorDto);

            if (!validator_result.IsValid)
                return new PaymentInitializeMediatorResponse(validator_result.Errors.ToString());

            var payment = new Payment()
            {
                OrderID = request.OrderID,
                PaymentProvider = request.PaymentPlatform,
                Price = request.Money
            };

            payment.SetStatus(EPaymentStatus.Initializing);

            await _paymentsContext.AddAsync(payment);
            await _paymentsContext.SaveChangesAsync();

            return new PaymentInitializeMediatorResponse(payment.ID);
        }
    }
}