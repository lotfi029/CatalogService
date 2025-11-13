namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class CategoryVariantConfiguration : IEntityTypeConfiguration<CategoryVariantAttribute>
{
    public void Configure(EntityTypeBuilder<CategoryVariantAttribute> builder)
    {
        builder.ToTable("category_variant_attributes");

        builder.Property(cv => cv.CategoryId)
            .HasColumnName("category_id");
        builder.Property(cv => cv.VariantAttributeId)
            .HasColumnName("variant_attribute_id");

        builder.HasKey(cva => new { cva.CategoryId, cva.VariantAttributeId });
        
        builder.HasOne(cva => cva.Category)
            .WithMany(c => c.CategoryVariantAttributes)
            .HasForeignKey(cva => cva.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cva => cva.VariantAttribute)
            .WithMany(va => va.CategoryVariantAttributes)
            .HasForeignKey(cva => cva.VariantAttributeId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Property(cva => cva.IsRequired)
            .HasColumnName("is_required")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(cva => cva.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired()
            .HasDefaultValue((short)0);

        builder.Property(cva => cva.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(cva => cva.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsRequired(false);
    }
}