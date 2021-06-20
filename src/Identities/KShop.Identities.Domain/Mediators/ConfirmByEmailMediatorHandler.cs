using FluentValidation;
using KShop.Shared.Domain.Contracts;
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

    public class ConfirmByEmailMediatorResponse : BaseResponse
    {
    }
    public class ConfirmByEmailMediatorRequest : IRequest<ConfirmByEmailMediatorResponse>
    {
        public string Email { get; set; }
        public string ConfirmToken { get; set; }
    }
    public class ConfirmByEmailMediatorHandler : IRequestHandler<ConfirmByEmailMediatorRequest, ConfirmByEmailMediatorResponse>
    {
        private readonly ILogger<ConfirmByEmailMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;

        public ConfirmByEmailMediatorHandler(ILogger<ConfirmByEmailMediatorHandler> logger, IdentityUserManager userMan)
        {
            _logger = logger;
            _userMan = userMan;
        }

        public async Task<ConfirmByEmailMediatorResponse> Handle(ConfirmByEmailMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserByEmailAsync(request.Email);
            var response = await _userMan.ConfirmEmailAsync(user, request.ConfirmToken);
            return new ConfirmByEmailMediatorResponse()
            {
                ErrorMessage = string.Join(" | ", response.Errors.Select(e => e.Description))
            };
        }
    }
}
