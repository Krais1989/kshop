using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence
{
    /// <summary>
    /// Контекст для миграций
    /// </summary>
    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var constr = "Server=127.0.0.1;Port=3307;Database=db_orders;Uid=asd;Pwd=asd;";
            Console.WriteLine($"Design context: {constr}");
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            return new OrderContext(optionsBuilder.Options);
        }
    }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
        }

    }
}
