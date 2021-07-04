using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Linq;

namespace KShop.Shipments.Persistence
{
    /// <summary>
    /// Контекст для миграций
    /// </summary>
    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<ShipmentContext>
    {
        public ShipmentContext CreateDbContext(string[] args)
        {
            var constr = "Server=127.0.0.1;Port=3309;Database=db_shipments;Uid=asd;Pwd=asd;";
            Console.WriteLine($"Design context: {constr}");
            var optionsBuilder = new DbContextOptionsBuilder<ShipmentContext>();
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            return new ShipmentContext(optionsBuilder.Options);
        }
    }

    public class ShipmentContext : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }


        public ShipmentContext()
        {
        }

        public ShipmentContext(DbContextOptions<ShipmentContext> options) : base(options)
        {
            Database.EnsureCreated();
            //if (Database.GetPendingMigrations().Any())
            //    Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentContext).Assembly);
        }
    }
}
