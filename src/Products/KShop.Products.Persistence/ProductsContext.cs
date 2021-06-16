
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
    /// <summary>
    /// Контекст для миграций
    /// </summary>
    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<ProductsContext>
    {
        public ProductsContext CreateDbContext(string[] args)
        {
            var constr = "Server=127.0.0.1;Port=3306;Database=db_products;Uid=asd;Pwd=asd;";
            Console.WriteLine($"Design context: {constr}");
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            return new ProductsContext(optionsBuilder.Options);
        }
    }

    public class ProductsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPosition> ProductPositions { get; set; }
        public DbSet<ProductReserve> ProductReserves { get; set; }

        public ProductsContext()
        {
        }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        private static ILoggerFactory ContextLoggerFactory
            => LoggerFactory.Create(b => b.AddFilter("", LogLevel.Debug));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsContext).Assembly);
        }

    }
}
