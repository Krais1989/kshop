using FluentValidation;
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

    public class PaymentCreateMediatorResponse
    {
    }
    public class PaymentCreateMediatorRequest : IRequest<PaymentCreateMediatorResponse>
    {
        public Guid OrderID { get; set; }
        public decimal Price { get; set; }
    }
    public class PaymentCreateMediatorHandler : IRequestHandler<PaymentCreateMediatorRequest, PaymentCreateMediatorResponse>
    {
        private readonly ILogger<PaymentCreateMediatorHandler> _logger;
        private readonly IValidator<PaymentCreateValidatorDto> _validator;
        private readonly PaymentsContext _paymentsContext;

        public PaymentCreateMediatorHandler(ILogger<PaymentCreateMediatorHandler> logger, IValidator<PaymentCreateValidatorDto> validator, PaymentsContext paymentsContext)
        {
            _logger = logger;
            _validator = validator;
            _paymentsContext = paymentsContext;
        }

        public async Task<PaymentCreateMediatorResponse> Handle(PaymentCreateMediatorRequest request, CancellationToken cancellationToken)
        {
            var validatorDto = new PaymentCreateValidatorDto() { };
            _validator.Validate(validatorDto);

            var payment = new Payment()
            {
                ID = request.OrderID,
                StatusDate = DateTime.UtcNow,
                Status = Payment.EStatus.Pending,
                Logs = new List<PaymentLog> {
                        new PaymentLog()
                        {
                            ModifyDate = DateTime.UtcNow,
                            Status = Payment.EStatus.Pending
                        }
                    }
            };

            await _paymentsContext.Payments.AddAsync(payment);
            await _paymentsContext.SaveChangesAsync();

            return new PaymentCreateMediatorResponse();
        }
    }
}
