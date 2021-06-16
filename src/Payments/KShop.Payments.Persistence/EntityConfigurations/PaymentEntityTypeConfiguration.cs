using KShop.Shared.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(e => e.ID);
            builder.HasMany(e => e.Logs).WithOne(pos => pos.Payment).HasForeignKey(pos => pos.PaymentID);
            builder.OwnsOne(e => e.Money, e => {
                e.Property(p => p.Price)
                    .HasColumnName("Price")
                    .HasDefaultValue<decimal>(0.0);
                e.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasDefaultValue(Money.CurrencySign.RUB);
            });
            //builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            //builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);
        }
    }

    public class PaymentLogEntityTypeConfiguration : IEntityTypeConfiguration<PaymentLog>
    {
        public void Configure(EntityTypeBuilder<PaymentLog> builder)
        {
            builder.HasKey(e => e.ID);

            //builder.HasMany(e => e.Positions).WithOne(pos => pos.Product).HasForeignKey(pos => pos.ProductID);
            //builder.HasMany(e => e.Reserves).WithOne(r => r.Product).HasForeignKey(r => r.ProductID);
        }
    }
}
