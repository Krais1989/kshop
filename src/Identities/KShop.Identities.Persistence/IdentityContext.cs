using KShop.Identities.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace KShop.Identities.Persistence
{
    /// <summary>
    /// Контекст для миграций
    /// </summary>
    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var constr = "Server=127.0.0.1;Port=3310;Database=db_identities;Uid=asd;Pwd=asd;";
            Console.WriteLine($"Design context: {constr}");
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            return new IdentityContext(optionsBuilder.Options);
        }
    }

    public class IdentityContext : IdentityDbContext<User, Role, uint, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public IdentityContext()
        {
        }

        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Настройка identity-пользователя */
            builder.Entity<User>(e =>
            {
                e.ToTable("AspNetUsers");
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                e.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

                e.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                e.Property(u => u.Status).HasColumnType("TINYINT");

                e.HasMany(u => u.UserClaims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                //e.HasMany(u => u.UserLogins).WithOne(ul => ul.User).HasForeignKey(ul => ul.UserId);
                //e.HasMany(u => u.UserTokens).WithOne(ut => ut.User).HasForeignKey(ut => ut.UserId);
                //e.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);
            });

            /* Настройка identity-роли */
            builder.Entity<Role>(e =>
            {
                e.ToTable("AspNetRoles");
                e.HasKey(r => r.Id);
                e.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

                e.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                //e.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId);
                //e.HasMany(r => r.RoleClaims).WithOne(rc => rc.Role).HasForeignKey(rc => rc.RoleId);
            });

            /* Настройка пользователь-утверждение */
            builder.Entity<UserClaim>(e =>
            {
                e.ToTable("AspNetUserClaims");
                e.HasKey(uc => new { uc.Id, uc.UserId });
            });

            /* Настройка пользователь-вход */
            builder.Entity<UserLogin>(e =>
            {
                e.ToTable("AspNetUserLogins");
                e.HasKey(x => new { x.LoginProvider, x.ProviderKey });
            });

            /* Настройка пользователь-токен */
            builder.Entity<UserToken>(e =>
            {
                e.ToTable("AspNetUserTokens");
                e.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            });

            /* Настройка роль-утверждение */
            builder.Entity<RoleClaim>(e =>
            {
                e.ToTable("AspNetRoleClaims");
                e.HasKey(rc => rc.Id);
            });

            /* Настройка пользователь-роль */
            builder.Entity<UserRole>(e =>
            {
                e.ToTable("AspNetUserRoles");
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });
        }
    }
}
