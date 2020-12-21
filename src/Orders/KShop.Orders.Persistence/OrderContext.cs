using KShop.Orders.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPosition> OrderPositions { get; set; }

        public OrderContext()
        {
        }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3307;Database=db_orders;Uid=asd;Pwd=asd;", new MySqlServerVersion(new Version(8, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
        }

    }
}
