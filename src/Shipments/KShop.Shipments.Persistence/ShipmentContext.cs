using KShop.Shipments.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace KShop.Shipments.Persistence
{
    public class ShipmentContext : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }


        public ShipmentContext()
        {

        }

        public ShipmentContext(DbContextOptions<ShipmentContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseMySql("Server=127.0.0.1;Port=3307;Database=db_orders;Uid=asd;Pwd=asd;", new MySqlServerVersion(new Version(8, 0)));

            var constr = "Server=127.0.0.1;Port=3309;Database=db_shipments;Uid=asd;Pwd=asd;";
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentContext).Assembly);
        }
    }
}
