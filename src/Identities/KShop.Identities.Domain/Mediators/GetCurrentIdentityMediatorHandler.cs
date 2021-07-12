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

    public class GetCurrentIdentityMediatorResponse
    {
        public GetCurrentIdentityMediatorResponse(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
    public class GetCurrentIdentityMediatorRequest : IRequest<GetCurrentIdentityMediatorResponse>
    {
        public GetCurrentIdentityMediatorRequest(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; private set; }
    }
    public class GetCurrentIdentityMediatorHandler : IRequestHandler<GetCurrentIdentityMediatorRequest, GetCurrentIdentityMediatorResponse>
    {
        private readonly ILogger<GetCurrentIdentityMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;
        private readonly IdentitySignInManager _signMan;

        public GetCurrentIdentityMediatorHandler(ILogger<GetCurrentIdentityMediatorHandler> logger, IdentityUserManager userMan, IdentitySignInManager signMan)
        {
            _logger = logger;
            _userMan = userMan;
            _signMan = signMan;
        }

        public async Task<GetCurrentIdentityMediatorResponse> Handle(GetCurrentIdentityMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserAsync(request.User);
            return new GetCurrentIdentityMediatorResponse(user.Email);
        }
    }
}
