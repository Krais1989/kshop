
using KShop.Products.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence.EntityConfigurations
{
    public class ProductReserveEntityTypeConfiguration : IEntityTypeConfiguration<ProductReserve>
    {
        public void Configure(EntityTypeBuilder<ProductReserve> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
