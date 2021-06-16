using FluentValidation;
using KShop.Identities.Persistence;
using KShop.Shared.Domain;
using KShop.Shared.Domain.Contracts;
using MassTransit;
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
    public class AccountRegistrationMediatorResponse : BaseResponse
    {
        public uint ID { get; set; }
    }
    public class AccountRegistrationMediatorRequest : IRequest<AccountRegistrationMediatorResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class AccountRegistrationMediatorHandler : IRequestHandler<AccountRegistrationMediatorRequest, AccountRegistrationMediatorResponse>
    {
        private readonly ILogger<AccountRegistrationMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;
        private readonly IPublishEndpoint _publishEndpoint;

        public AccountRegistrationMediatorHandler(
            ILogger<AccountRegistrationMediatorHandler> logger,
            IdentityUserManager userMan,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _userMan = userMan;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<AccountRegistrationMediatorResponse> Handle(AccountRegistrationMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var createResult = await _userMan.CreateAsync(user, request.Password);

            if (createResult.Succeeded)
                return new AccountRegistrationMediatorResponse { ID = user.Id };
            else
                return new AccountRegistrationMediatorResponse { ErrorMessage = string.Join("\n", createResult.Errors.Select(e => e.Description)) };
        }
    }
}
