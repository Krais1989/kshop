using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KShop.Shared.Authentication
{
    public class JwtFactory : IJWTFactory
    {
        private readonly JwtSettings _jwtSettings;

        public JwtFactory(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string Generate(IEnumerable<Claim> claims)
        {
            /* Генерация токена */
            var jwtToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessExpiration),
                    signingCredentials: _jwtSettings.GetSigningCredentials() // Подпись
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}