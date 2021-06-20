using KShop.Identities.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KShop.Identities.Domain
{

    public class IdentityUserManager : UserManager<User>
    {
        public IdentityUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger) : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
        }


        public async override Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            var result = await Users.SingleOrDefaultAsync(e => e.Id == principal.GetID());
            return result;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var result = await Users.SingleOrDefaultAsync(e => e.Email == email);
            return result;
        }

        //public async virtual Task<User> GetUserAsync<TProperty>(ClaimsPrincipal principal, Expression<Func<User, TProperty>> navigationPropertyPath = null)
        //{
        //    var result = await (navigationPropertyPath == null ? Users : Users.Include(e => navigationPropertyPath))
        //        .SingleOrDefaultAsync(e => e.UserName == principal.GetName());

        //    return result;
        //}
    }
}
