using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Identities.Domain
{

    public class AccountChangePasswordMediatorResponse
    {
    }
    public class AccountChangePasswordMediatorRequest : IRequest<AccountChangePasswordMediatorResponse>
    {
    }
    public class AccountChangePasswordMediatorHandler : IRequestHandler<AccountChangePasswordMediatorRequest, AccountChangePasswordMediatorResponse>
    {
        private readonly ILogger<AccountChangePasswordMediatorHandler> _logger;

        public AccountChangePasswordMediatorHandler(ILogger<AccountChangePasswordMediatorHandler> logger)
        {
            _logger = logger;
        }

        public async Task<AccountChangePasswordMediatorResponse> Handle(AccountChangePasswordMediatorRequest request, CancellationToken cancellationToken)
        {
            return new AccountChangePasswordMediatorResponse();
        }
    }
}
