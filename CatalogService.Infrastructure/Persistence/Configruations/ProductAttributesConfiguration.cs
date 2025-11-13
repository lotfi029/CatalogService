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

        builder.Property(pa => pa.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(pa => pa.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsRequired(false);

        builder.HasOne(pa => pa.Product)
            .WithMany(p => p.ProductAttributes)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(pa => pa.Attribute)
            .WithMany(a => a.ProductAttributes)
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
