using CatalogService.Domain.Contants;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class ProductVariantValueConfiguration : IEntityTypeConfiguration<ProductVariantValue>
{
    public void Configure(EntityTypeBuilder<ProductVariantValue> builder)
    {
        builder.HasKey(pvv => pvv.Id);

        builder.Property(pvv => pvv.Id)
            .HasColumnName("id");

        builder.Property(pvv => pvv.Value)
            .HasMaxLength(450)
            .HasColumnName("value");

        builder.Property(pvv => pvv.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(pvv => pvv.IsDeleted)
            .HasColumnName("is_deleted");

        builder.HasOne(e => e.ProductVariant)
            .WithMany(pvv => pvv.Values)
            .HasForeignKey(pvv => pvv.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(pvv => pvv.ProductVariantId)
            .HasColumnName("product_variant_id");

        builder.HasOne(e => e.VariantAttributeDefinition)
            .WithMany()
            .HasForeignKey(pvv => pvv.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(pvv => pvv.VariantAttributeId)
            .HasColumnName("variant_attribute_id");

        builder.HasQueryFilter(
            QueryFilterConsts.SoftDeleteFilter,
            x =>
                !x.IsDeleted && 
                !x.ProductVariant.IsDeleted && 
                !x.VariantAttributeDefinition.IsDeleted);
    }
}