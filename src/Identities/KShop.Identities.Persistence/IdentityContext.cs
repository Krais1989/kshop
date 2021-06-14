using KShop.Identities.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace KShop.Identities.Persistence
{
    
    public class IdentityContext : IdentityDbContext<User, Role, uint, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
    }
}
