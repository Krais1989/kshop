using KShop.Products.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var constr = "Server=127.0.0.1;Port=3306;Database=db_products;Uid=asd;Pwd=asd;";
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            //.UseLoggerFactory(ContextLoggerFactory);

            //optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=db_catalogues;Uid=asd;Pwd=asd;", new MySqlServerVersion(new Version(8, 0)));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsContext).Assembly);
        }

    }
}
