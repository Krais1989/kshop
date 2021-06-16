using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Identities.Persistence
{

    public class User : IdentityUser<uint>
    {
        public EUserStatus Status { get; set; }

        public ICollection<UserClaim> UserClaims { get; set; }
        public ICollection<UserLogin> UserLogins { get; set; }
        public ICollection<UserToken> UserTokens { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
