

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence
{
    public class ProductReserveEntityTypeConfiguration : IEntityTypeConfiguration<ProductReserve>
    {
        public void Configure(EntityTypeBuilder<ProductReserve> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
