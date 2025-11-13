namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class AttributeConfiguration : BaseEntityConfiguration<Core.Entities.Attribute>
{
    public override void Configure(EntityTypeBuilder<Core.Entities.Attribute> builder)
    {
        base.Configure(builder);
        builder.ToTable("attributes");

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(200);

        builder.Property(a => a.Code)
            .HasColumnName("code")
            .HasMaxLength(100);
        builder.HasIndex(a => a.Code)
            .IsUnique()
            .HasDatabaseName("idx_attributes_code");

        builder.Property(a => a.Type)
            .HasColumnName("type")
            .HasConversion<short>();
        builder.HasIndex(a => a.Type)
            .HasDatabaseName("idx_attributes_type");


        builder.Property(a => a.IsFilterable)
            .HasColumnName("is_filterable")
            .HasDefaultValue(false);
        builder.HasIndex(a => a.IsFilterable)
            .HasDatabaseName("idx_attributes_is_filterable")
            .HasFilter("is_filterable = true");

        builder.Property(a => a.IsSearchable)
            .HasColumnName("is_searchable")
            .HasDefaultValue(false);

        builder.Property(a => a.Options)
            .HasColumnName("options")
            .HasColumnType("jsonb")
            .IsRequired(false)
            .HasDefaultValue("'[]'::jsonb");

        builder.ToTable(a => a.HasCheckConstraint(
            "chk_attributes_type",
            "type > 0 AND type <= 6"
            ));
    }
}