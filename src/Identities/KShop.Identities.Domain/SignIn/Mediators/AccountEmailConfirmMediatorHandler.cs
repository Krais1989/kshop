using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Identities.Domain.SignIn.Mediators
{

    public class AccountEmailConfirmMediatorResponse
    {
    }
    public class AccountEmailConfirmMediatorRequest : IRequest<AccountEmailConfirmMediatorResponse>
    {
    }
    public class AccountEmailConfirmMediatorHandler : IRequestHandler<AccountEmailConfirmMediatorRequest, AccountEmailConfirmMediatorResponse>
    {
        private readonly ILogger<AccountEmailConfirmMediatorHandler> _logger;

        public AccountEmailConfirmMediatorHandler(ILogger<AccountEmailConfirmMediatorHandler> logger)
        {
            _logger = logger;
        }

        public async Task<AccountEmailConfirmMediatorResponse> Handle(AccountEmailConfirmMediatorRequest request, CancellationToken cancellationToken)
        {
            return new AccountEmailConfirmMediatorResponse();
        }
    }
}
