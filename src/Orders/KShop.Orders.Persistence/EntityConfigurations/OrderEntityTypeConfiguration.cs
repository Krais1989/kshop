using KShop.Orders.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Orders.Persistence.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.ID);
            builder.HasMany(e => e.Positions).WithOne(p => p.Order).HasForeignKey(p => p.OrderID);

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
