using KShop.Identities.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KShop.Identities.Domain
{

    public class IdentityRoleManager : RoleManager<Role>
    {
        public IdentityRoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger) : base(
                store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
        }
    }
}
