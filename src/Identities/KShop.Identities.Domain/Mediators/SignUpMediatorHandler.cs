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
    public class SignUpMediatorResponse : BaseResponse
    {
        public uint ID { get; set; }
    }
    public class SignUpMediatorRequest : IRequest<SignUpMediatorResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class SignUpMediatorHandler : IRequestHandler<SignUpMediatorRequest, SignUpMediatorResponse>
    {
        private readonly ILogger<SignUpMediatorHandler> _logger;
        private readonly IdentityUserManager _userMan;
        private readonly IPublishEndpoint _publishEndpoint;

        public SignUpMediatorHandler(
            ILogger<SignUpMediatorHandler> logger,
            IdentityUserManager userMan,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _userMan = userMan;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<SignUpMediatorResponse> Handle(SignUpMediatorRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var createResult = await _userMan.CreateAsync(user, request.Password);

            if (createResult.Succeeded)
                return new SignUpMediatorResponse { ID = user.Id };
            else
                return new SignUpMediatorResponse { ErrorMessage = string.Join("\n", createResult.Errors.Select(e => e.Description)) };
        }
    }
}
