using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence
{
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
