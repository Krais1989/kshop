using KShop.Catalogues.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Catalogues.Persistence.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ID);

            builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);

        }
    }
}
