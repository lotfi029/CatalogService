using CatalogService.Domain.Contants;

namespace CatalogService.Infrastructure.Persistence.Configruations;

public sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategories>
{
    public void Configure(EntityTypeBuilder<ProductCategories> builder)
    {
        builder.ToTable("product_categories");

        builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });
        builder.Property(e => e.ProductId)
            .HasColumnName("product_id");
        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id");

        builder.Property(pc => pc.IsPrimary)
            .HasColumnName("is_primary")
            .HasDefaultValue(false);

        builder.Property(pc => pc.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(pc => pc.Product)
            .WithMany(p => p.Categories)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(pc => pc.Category)
            .WithMany()
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(pc => pc.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(QueryFilterConsts.SoftDeleteFilter,
            pc => !pc.Category.IsDeleted &&
            !pc.IsDeleted &&
            !pc.Product.IsDeleted);
    }
}