using KShop.Identities.Persistence;
using KShop.Identities.Persistence.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KShop.Identities.Domain
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddKNewsIdentity(this IServiceCollection services, IConfiguration config, string configKey = "IdentityOptions")
        {
            var confSection = config.GetSection(configKey);
            services.Configure<IdentityOptions>(confSection);
            var idOpts = confSection.Get<IdentityOptions>();
            services.AddIdentityCore<User>(opt => config.Bind(configKey, opt))
                .AddRoles<Role>()
                .AddUserStore<UserStore<User, Role, IdentityContext, uint, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>>() // required
                .AddRoleStore<RoleStore<Role, IdentityContext, uint, UserRole, RoleClaim>>() // required
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, Role>>()
                .AddUserManager<IdentityUserManager>()
                .AddRoleManager<IdentityRoleManager>()
                .AddSignInManager<IdentitySignInManager>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
    }
}
