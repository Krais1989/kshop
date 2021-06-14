using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Identities.Domain.SignIn.Mediators
{

    public class AccountGetCurrentMediatorResponse
    {
        public string Email { get; set; }
    }
    public class AccountGetCurrentMediatorRequest : IRequest<AccountGetCurrentMediatorResponse>
    {
        public ClaimsPrincipal User { get; set; }
    }
    public class AccountGetCurrentMediatorHandler : IRequestHandler<AccountGetCurrentMediatorRequest, AccountGetCurrentMediatorResponse>
    {
        private readonly ILogger<AccountGetCurrentMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;

        public AccountGetCurrentMediatorHandler(ILogger<AccountGetCurrentMediatorHandler> logger, IdentityUserManager userMan)
        {
            _logger = logger;
            _userMan = userMan;
        }

        public async Task<AccountGetCurrentMediatorResponse> Handle(AccountGetCurrentMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserAsync(request.User);
            return new AccountGetCurrentMediatorResponse() { Email = user.Email };
        }
    }
}
