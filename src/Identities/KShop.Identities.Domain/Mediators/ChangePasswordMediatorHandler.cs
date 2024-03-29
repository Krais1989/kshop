﻿using FluentValidation;
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

    public class ChangePasswordMediatorResponse : BaseResponse
    {
        public ChangePasswordMediatorResponse(string error) : base(error)
        {
        }

        public ChangePasswordMediatorResponse(bool isSuccess = true) : base(isSuccess)
        {
        }
    }
    public class ChangePasswordMediatorRequest : IRequest<ChangePasswordMediatorResponse>
    {
        public ChangePasswordMediatorRequest(ClaimsPrincipal user, string oldPassword, string newPassword)
        {
            User = user;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public ClaimsPrincipal User { get; private set; }
        public string OldPassword { get; private set; }
        public string NewPassword { get; private set; }
    }
    public class ChangePasswordMediatorHandler : IRequestHandler<ChangePasswordMediatorRequest, ChangePasswordMediatorResponse>
    {
        private readonly ILogger<ChangePasswordMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;

        public ChangePasswordMediatorHandler(ILogger<ChangePasswordMediatorHandler> logger, IdentityUserManager userMan)
        {
            _logger = logger;
            _userMan = userMan;
        }

        public async Task<ChangePasswordMediatorResponse> Handle(ChangePasswordMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.GetUserAsync(request.User);
            var result = await _userMan.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
                return new ChangePasswordMediatorResponse(result.Errors.First().Description);
            return new ChangePasswordMediatorResponse();
        }
    }
}
