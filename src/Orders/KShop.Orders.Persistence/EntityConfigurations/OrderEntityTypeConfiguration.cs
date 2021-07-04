using KShop.Shared.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.ID);
            builder.HasMany(o => o.Positions).WithOne(op => op.Order).HasForeignKey(op => op.OrderID);
            builder.HasMany(o => o.Logs).WithOne(l => l.Order).HasForeignKey(l => l.OrderID);
            //builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            //builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);
            builder.OwnsOne(e => e.Price, e => {
                e.Property(p => p.Price)
                    .HasColumnName("Price")
                    .HasDefaultValue<decimal>(0.0);
                e.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasDefaultValue(Money.CurrencySign.RUB);
            });
        }
    }

    public class OrderLogEntityTypeConfiguration : IEntityTypeConfiguration<OrderLog>
    {
        public void Configure(EntityTypeBuilder<OrderLog> builder)
        {
            builder.HasKey(e => e.ID);

            //builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            //builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);
        }
    }

    public class OrderPositionTypeConfiguration : IEntityTypeConfiguration<OrderPosition>
    {
        public void Configure(EntityTypeBuilder<OrderPosition> builder)
        {
            builder.HasKey(e => e.ID);

            //builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            //builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);
        }
    }
}
