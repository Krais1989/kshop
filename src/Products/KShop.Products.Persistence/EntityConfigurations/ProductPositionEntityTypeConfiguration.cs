

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence
{
    public class ProductPositionEntityTypeConfiguration : IEntityTypeConfiguration<ProductPosition>
    {
        public void Configure(EntityTypeBuilder<ProductPosition> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
