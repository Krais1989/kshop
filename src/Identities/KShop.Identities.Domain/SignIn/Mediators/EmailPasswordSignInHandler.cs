using FluentValidation;
using KShop.Shared.Authentication;
using KShop.Shared.Domain;
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
    public class EmailPasswordSignInResponse : BaseResponse
    {
        public string Login { get; set; }
        public string Token { get; set; }
    }
    public class EmailPasswordSignInHandlerRequest : IRequest<EmailPasswordSignInResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class EmailPasswordSignInHandler : IRequestHandler<EmailPasswordSignInHandlerRequest, EmailPasswordSignInResponse>
    {
        private readonly ILogger<EmailPasswordSignInHandler> _logger;
        private readonly IdentityUserManager _userMan;
        private readonly IJWTFactory _jwtFactory;

        public EmailPasswordSignInHandler(ILogger<EmailPasswordSignInHandler> logger, IdentityUserManager userMan, IJWTFactory jwtFactory)
        {
            _logger = logger;
            _userMan = userMan;
            _jwtFactory = jwtFactory;
        }

        public async Task<EmailPasswordSignInResponse> Handle(EmailPasswordSignInHandlerRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMan.FindByEmailAsync(request.Email);
            if (user == null)
                return new EmailPasswordSignInResponse() { ErrorMessage = "Invalid Email or Password" };

            var passCheck = await _userMan.CheckPasswordAsync(user, request.Password);
            if (!passCheck)
                return new EmailPasswordSignInResponse() { ErrorMessage = "Invalid Email or Password" };


            var tokenClaims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            return new EmailPasswordSignInResponse()
            {
                Login = user.Email,
                Token = _jwtFactory.Generate(tokenClaims)
            };
        }
    }


}
