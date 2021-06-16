using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shared.Authentication
{
    public class JwtSettings 
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }

        /// <summary>
        /// Симметричный ключ
        /// </summary>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        }

        /// <summary>
        /// Подпись ключа алгоритмом HmacSha256
        /// </summary>
        public SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        }
    }
}
