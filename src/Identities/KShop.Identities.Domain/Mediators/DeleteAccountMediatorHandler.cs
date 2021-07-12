using FluentValidation;
using KShop.Shared.Domain.Contracts;
using MediatR;
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

    public class DeleteAccountMediatorResponse : BaseResponse
    {
    }
    public class DeleteAccountMediatorRequest : IRequest<DeleteAccountMediatorResponse>
    {
        public DeleteAccountMediatorRequest(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal User { get; private set; }
    }
    public class DeleteAccountMediatorHandler : IRequestHandler<DeleteAccountMediatorRequest, DeleteAccountMediatorResponse>
    {
        private readonly ILogger<DeleteAccountMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;

        public DeleteAccountMediatorHandler(ILogger<DeleteAccountMediatorHandler> logger, IdentityUserManager userMan)
        {
            _logger = logger;
            _userMan = userMan;
        }

        public async Task<DeleteAccountMediatorResponse> Handle(DeleteAccountMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserAsync(request.User);
            var result = await _userMan.DeleteAsync(user);
            return new DeleteAccountMediatorResponse()
            {
                ErrorMessage = result.Succeeded ? null : string.Join(" | ", result.Errors.Select(e => e.Description))
            };
        }
    }
}
