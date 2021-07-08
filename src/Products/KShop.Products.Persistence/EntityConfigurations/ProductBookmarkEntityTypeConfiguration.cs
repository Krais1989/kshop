using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KShop.Products.Persistence
{
    public class ProductBookmarkEntityTypeConfiguration : IEntityTypeConfiguration<ProductBookmark>
    {
        public void Configure(EntityTypeBuilder<ProductBookmark> builder)
        {
            builder.HasKey(e => new { e.ProductID, e.CustomerID });
            builder.HasOne(e => e.Product).WithMany(e => e.ProductFavorits).HasForeignKey(e => e.ProductID);
        }
    }
}
