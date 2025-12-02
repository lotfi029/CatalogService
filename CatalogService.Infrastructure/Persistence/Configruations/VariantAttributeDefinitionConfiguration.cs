using CatalogService.Domain.JsonProperties;
using System.Text.Json;

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

        builder.OwnsOne(vtd => vtd.Datatype, dataType =>
        {
            dataType.Property(d => d.Datatype)
                .HasColumnName("data_type")
                .HasConversion<short>()
                .IsRequired();

            dataType.Property(d => d.DatatypeName)
                .HasColumnName("data_type_name")
                .HasMaxLength(10)
                .IsRequired();

            dataType.WithOwner();
        });
        builder.Property(vtd => vtd.AffectsInventory)
            .HasColumnName("affects_inventory")
            .HasDefaultValue(false);

        builder.Property(vtd => vtd.AllowedValues)
            .HasColumnName("allowed_values")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<AllowedValuesJson>(v))
            .IsRequired(false);

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("idx_variant_attribute_definitions_is_active");
    }
}
