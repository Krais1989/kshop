using KShop.Shared.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ID);

            builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);

            builder.OwnsOne(e => e.Money, e =>
            {
                e.Property(p => p.Price)
                    .HasColumnName("Price")
                    .HasDefaultValue<decimal>(0.0);
                e.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasDefaultValue(Money.CurrencySign.RUB);
            });
        }
    }

    public class ProductAttributeEntityTypeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.HasKey(e => new { e.ProductID, e.AttributeID });
            builder
                .HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAttributes)
                .HasForeignKey(p => p.ProductID);
            builder
                .HasOne(pa => pa.Attribute)
                .WithMany(p => p.ProductAttributes)
                .HasForeignKey(p => p.AttributeID);
        }
    }
}
