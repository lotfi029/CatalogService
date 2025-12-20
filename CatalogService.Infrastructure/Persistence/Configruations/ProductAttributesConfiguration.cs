using CatalogService.Domain.Contants;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class ProductAttributesConfiguration : IEntityTypeConfiguration<ProductAttributes>
{
    public void Configure(EntityTypeBuilder<ProductAttributes> builder)
    {
        builder.ToTable("product_attributes");

        builder.HasKey(pt => new { pt.ProductId, pt.AttributeId });
        builder.Property(pa => pa.ProductId)
            .HasColumnName("product_id");
        builder.Property(pa => pa.AttributeId)
            .HasColumnName("attribute_id");

        builder.Property(pa => pa.Value)
            .HasColumnName("value")
            .HasMaxLength(200);
        builder.HasIndex(pa => pa.Value)
            .HasDatabaseName("idx_product_attributes_value");

        builder.Property(pa => pa.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(pa => pa.Product)
            .WithMany(p => p.Attributes)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pa => pa.Attribute)
            .WithMany()
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(cva => cva.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(QueryFilterConsts.SoftDeleteFilter,
            pa => !pa.Product.IsDeleted &&
            !pa.IsDeleted &&
            !pa.Attribute.IsDeleted);

    }
}
