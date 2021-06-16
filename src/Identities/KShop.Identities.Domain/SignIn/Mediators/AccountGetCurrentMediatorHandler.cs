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

namespace KShop.Identities.Domain
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
        private readonly IdentitySignInManager _signMan;

        public AccountGetCurrentMediatorHandler(ILogger<AccountGetCurrentMediatorHandler> logger, IdentityUserManager userMan, IdentitySignInManager signMan)
        {
            _logger = logger;
            _userMan = userMan;
            _signMan = signMan;
        }

        public async Task<AccountGetCurrentMediatorResponse> Handle(AccountGetCurrentMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserAsync(request.User);
            return new AccountGetCurrentMediatorResponse() { Email = user.Email };
        }
    }
}
