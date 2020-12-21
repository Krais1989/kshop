using KShop.Catalogues.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Catalogues.Persistence.EntityConfigurations
{
    public class ProductReserveEntityTypeConfiguration : IEntityTypeConfiguration<ProductReserve>
    {
        public void Configure(EntityTypeBuilder<ProductReserve> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
