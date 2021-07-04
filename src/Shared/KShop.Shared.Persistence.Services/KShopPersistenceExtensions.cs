using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace KShop.Shared.Persistence.Services
{
    public static class KShopPersistenceExtensions
    {
        public static IApplicationBuilder UseDbContextPreparation<T>(this IApplicationBuilder app)
            where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<T>())
                {
                    context.PrepareDbContext();
                }
            }

            return app;
        }

        public static void PrepareDbContext<T>(this T db)
            where T : DbContext
        {
            db.Database.EnsureDeleted();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
