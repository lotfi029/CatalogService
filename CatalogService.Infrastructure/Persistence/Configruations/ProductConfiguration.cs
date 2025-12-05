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
            .HasDatabaseName("idx_products_status");

        builder.Property(p => p.VendorId)
            .HasColumnName("vendor_id")
            .IsRequired();

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("idx_products_is_active");

        builder.ToTable(p => p.HasCheckConstraint(
            "chk_products_status",
            "status IN (1, 2, 3, 4)"
            ));
    }
}
