namespace CatalogService.Infrastructure.Persistence.Configruations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .IsRequired(false);

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
            .IsRequired();
        builder.HasIndex(p => p.VendorId)
            .IsUnique()
            .HasDatabaseName("idx_products_vendor_id");

        builder.Property(p => p.SKU)
            .HasColumnName("sku")
            .HasMaxLength(100)
            .IsRequired(false);
        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("idx_products_sku");


        builder.OwnsOne(c => c.BasePrice, price =>
        {
            price.Property(e => e.Amount)
                .HasColumnName("price")
                .HasColumnType("DECIMAL(10, 2)")
                .IsRequired();

            price.Property(e => e.CurrencyType)
                .HasColumnName("price_currency")
                .HasMaxLength(5)
                .IsRequired();

            price.HasIndex(p => p.Amount)
                .HasDatabaseName("idx_products_price");
        });

        base.Configure(builder);
    }
}
