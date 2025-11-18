namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(500)
            .IsRequired();
        builder.HasIndex(p => p.Name)
            .HasMethod("gin")
            .IsTsVectorExpressionIndex("english");

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .IsRequired(false);
        builder.HasIndex(p => p.Description)
            .HasMethod("gin")
            .IsTsVectorExpressionIndex("english");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired();
        builder.HasIndex(e => e.Status)
            .IsUnique()
            .HasDatabaseName("idx_products_status");

        builder.Property(e => e.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(p => p.VendorId)
            .HasColumnName("vendor_id")
            .HasMaxLength(450)
            .IsRequired();
        builder.HasIndex(p => p.VendorId)
            .IsUnique()
            .HasDatabaseName("idx_products_vendor_id");

        builder.Property(p => p.SKU)
            .HasConversion(
                sku => sku!.Value,
                value => Sku.Create(value)!
            )
            .HasColumnName("sku")
            .HasMaxLength(100)
            .IsRequired(false);
        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("idx_products_sku");


        builder.ToTable(p => p.HasCheckConstraint(
            "chk_products_status",
            "status IN (1, 2, 3, 4)"
            ));
    }
}
