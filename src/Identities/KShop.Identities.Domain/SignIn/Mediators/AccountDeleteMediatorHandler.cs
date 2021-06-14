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

    public class AccountDeleteMediatorResponse
    {
    }
    public class AccountDeleteMediatorRequest : IRequest<AccountDeleteMediatorResponse>
    {
    }
    public class AccountDeleteMediatorHandler : IRequestHandler<AccountDeleteMediatorRequest, AccountDeleteMediatorResponse>
    {
        private readonly ILogger<AccountDeleteMediatorHandler> _logger;

        public AccountDeleteMediatorHandler(ILogger<AccountDeleteMediatorHandler> logger)
        {
            _logger = logger;
        }

        public async Task<AccountDeleteMediatorResponse> Handle(AccountDeleteMediatorRequest request, CancellationToken cancellationToken)
        {
            return new AccountDeleteMediatorResponse();
        }
    }
}
