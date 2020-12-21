using KShop.Catalogues.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Catalogues.Persistence
{
    public class CatalogueContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPosition> ProductPositions { get; set; }
        public DbSet<ProductReserve> ProductReserves { get; set; }

        public CatalogueContext()
        {
        }

        public CatalogueContext(DbContextOptions<CatalogueContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3306;Database=db_catalogue;Uid=asd;Pwd=asd;", new MySqlServerVersion(new Version(8, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogueContext).Assembly);
        }

    }
}
