using FluentValidation;
using KShop.Communications.Contracts;
using KShop.Identities.Persistence.Entities;
using MassTransit;
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
    public class AccountRegistrationMediatorResponse : BaseResponse
    {
        public ulong ID { get; set; }
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
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var createResult = await _userMan.CreateAsync(user, request.Password);

            return new AccountRegistrationMediatorResponse { ID = user.Id };
        }
    }
}
