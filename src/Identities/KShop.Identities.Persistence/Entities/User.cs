using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Identities.Persistence.Entities
{
    public class User : IdentityUser<uint>
    {
    }

    public class Role : IdentityRole<uint>
    {
    }

    public class RoleClaim : IdentityRoleClaim<uint>
    {
    }

    public class UserLogin : IdentityUserLogin<uint>
    {
    }

    public class UserClaim : IdentityUserClaim<uint>
    {
    }

    public class UserRole : IdentityUserRole<uint>
    {
    }

    public class UserToken : IdentityUserToken<uint>
    {
    }

}
