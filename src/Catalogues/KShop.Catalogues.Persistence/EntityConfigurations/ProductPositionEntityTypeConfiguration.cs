
using KShop.Products.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence.EntityConfigurations
{
    public class ProductPositionEntityTypeConfiguration : IEntityTypeConfiguration<ProductPosition>
    {
        public void Configure(EntityTypeBuilder<ProductPosition> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
