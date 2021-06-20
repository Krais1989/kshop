using FluentValidation;
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

    public class RefreshTokenResponse
    {
        public string NewToken { get; set; }
    }
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly ILogger<RefreshTokenHandler> _logger;
        //private readonly IValidator<RefreshTokenFluentValidatorDto> _validator;

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new RefreshTokenFluentValidatorDto() { };
            //_validator.Validate(validatorDto);
            return new RefreshTokenResponse();
        }
    }
}
