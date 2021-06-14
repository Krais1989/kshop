using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Identities.Domain
{
    public static class IdentityUserExtensions
    {
        //public static string GetUserId(this ClaimsPrincipal principal)
        //{
        //    return principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Id).Value;
        //}

        public static uint GetID(this ClaimsPrincipal principal)
        {
            return uint.Parse(principal.Claims.FirstOrDefault(e => e.Type == "id").Value);
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;
        }

        public static string GetName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name).Value;
        }
    }
}
