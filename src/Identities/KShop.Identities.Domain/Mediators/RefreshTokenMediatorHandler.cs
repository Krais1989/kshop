using FluentValidation;
using KShop.Shared.Authentication;
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
        public RefreshTokenResponse(string newToken)
        {
            NewToken = newToken;
        }

        public string NewToken { get; private set; }
    }
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        public RefreshTokenRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; private set; }
    }
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly ILogger<RefreshTokenHandler> _logger;
        private readonly IJWTFactory _factory;

        public RefreshTokenHandler(ILogger<RefreshTokenHandler> logger, IJWTFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        //private readonly IValidator<RefreshTokenFluentValidatorDto> _validator;

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            //var validatorDto = new RefreshTokenFluentValidatorDto() { };
            //_validator.Validate(validatorDto);

            // Получить ID пользователя
            // Сгенерировать новый токен на основе Claims

            throw new NotImplementedException();

            return new RefreshTokenResponse("");
        }
    }
}
