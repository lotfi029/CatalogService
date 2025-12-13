using CatalogService.Domain.JsonProperties;
using System.Dynamic;
using System.Text.Json;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class AttributeConfiguration : BaseEntityConfiguration<Domain.Entities.Attribute>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Attribute> builder)
    {
        base.Configure(builder);

        builder.ToTable("attributes");

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(100);

        builder.Property(a => a.Code)
            .HasColumnName("code")
            .HasMaxLength(100);
        builder.HasIndex(a => a.Code)
            .IsUnique()
            .HasDatabaseName("idx_attributes_code");

        builder.OwnsOne(vtd => vtd.OptionsType, dataType =>
        {
            dataType.Property(d => d.DataType)
                .HasColumnName("options_type")
                .HasConversion<short>()
                .IsRequired();

            dataType.Property(d => d.DataTypeName)
                .HasColumnName("options_type_name")
                .HasMaxLength(10)
                .IsRequired();

            dataType.WithOwner();
        });


        builder.Property(a => a.IsFilterable)
            .HasColumnName("is_filterable")
            .HasDefaultValue(false);
        builder.HasIndex(a => a.IsFilterable)
            .HasDatabaseName("idx_attributes_is_filterable");
        builder.Property(a => a.IsSearchable)
            .HasColumnName("is_searchable")
            .HasDefaultValue(false);
        builder.HasIndex(a => a.IsSearchable)
            .HasDatabaseName("idx_attributes_is_searchable");

        builder.Property(vtd => vtd.Options)
            .HasColumnName("options")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<ValuesJson>(v))
            .IsRequired(false);

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("idx_attributes_is_active");
    }
}