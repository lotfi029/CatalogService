namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class VariantAttributeDefinitionConfiguration : BaseEntityConfiguration<VariantAttributeDefinition>
{
    public override void Configure(EntityTypeBuilder<VariantAttributeDefinition> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("variant_attribute_definitions");

        builder.Property(vtd => vtd.Code)
            .HasColumnName("code")
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(vtd => vtd.Code)
            .IsUnique()
            .HasDatabaseName("idx_variant_attribute_definition_code");

        
        builder.Property(vtd => vtd.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(vtd => vtd.DataType)
            .HasColumnName("data_type")
            .HasMaxLength(200)
            .IsRequired();

        
        builder.Property(vtd => vtd.IsRequired)
            .HasColumnName("is_required")
            .HasDefaultValue(false);
        builder.Property(vtd => vtd.AffectsInventory)
            .HasColumnName("affects_inventory")
            .HasDefaultValue(false);
        builder.Property(vtd => vtd.AffectsPricing)
            .HasColumnName("affects_pricing")
            .HasDefaultValue(false);


        builder.Property(vtd => vtd.DisplayOrder)
            .HasColumnName("display_order")
            .IsRequired();

        builder.Property(vtd => vtd.AllowedValues)
            .HasColumnName("allowed_values")
            .HasColumnType("jsonb")
            .IsRequired();        
        builder.Property(vtd => vtd.ValidationRules)
            .HasColumnName("validation_rules")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
