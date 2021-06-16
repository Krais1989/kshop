using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace KShop.Shared.Authentication
{
    public interface IJWTFactory
    {
        string Generate(IEnumerable<Claim> Claims);
    }
}
