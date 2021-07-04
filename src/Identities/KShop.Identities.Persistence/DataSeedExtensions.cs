using KShop.Identities.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Identities.Persistence
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this ModelBuilder app)
        {
            var users_count = 10;

            var users = new List<User>();
            var ph = new PasswordHasher<User>();

            for (int i = 0; i < users_count; i++)
            {
                var u_id = (uint)i + 1;

                var user = new User()
                {
                    Id = u_id,
                    UserName = "asd",
                    Email = "asd@asd.ru",
                    PhoneNumber = "123123123"
                };                
                ph.HashPassword(user, "asd");
                users.Add(user);
            }

            app.Entity<User>().HasData(users);
        }
    }
}
