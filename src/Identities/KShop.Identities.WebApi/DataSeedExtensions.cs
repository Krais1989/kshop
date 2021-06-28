using KShop.Identities.Domain;
using KShop.Identities.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Identities.WebApi
{
    public static class DataSeedExtensions
    {
        public static IApplicationBuilder AddKShopTestData(this IApplicationBuilder app, IConfiguration config)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();
            using var user_manager = serviceScope.ServiceProvider.GetRequiredService<IdentityUserManager>();

            context.Database.EnsureDeleted();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();


            Action<string, string, string> createUser = (string email, string password, string phone) =>
            {
                var result = user_manager.CreateAsync(new User
                    {
                        UserName = email,
                        Email = email,
                        PhoneNumber = phone
                    }, password).GetAwaiter().GetResult();
            };

            createUser("asd@asd.ru", "asd", "123123123");

            for (int i = 0; i < 10; i++)
            {
                createUser($"asd{i}@asd.ru", "asd", $"1234567{i}");
            }

            return app;
        }
    }
}
