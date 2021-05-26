using KShop.Communications.Contracts.ValueObjects;
using KShop.Products.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Products.Persistence.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ID);

            builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);

            builder.OwnsOne(e => e.Money, e => {
                e.Property(p => p.Price)
                    .HasColumnName("Price")
                    .HasDefaultValue<decimal>(0.0);
                e.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasDefaultValue(Money.CurrencySign.RUB);
            });
        }
    }
}
