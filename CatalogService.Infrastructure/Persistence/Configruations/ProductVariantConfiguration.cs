using CatalogService.Domain.ValueObjects;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class ProductVariantConfiguration : BaseEntityConfiguration<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_variants");

        builder.Property(pv => pv.SKU)
            .HasConversion(
                sku => sku.Value,
                value => Sku.Create(value)!
            )
            .HasColumnName("sku")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(pv => pv.SKU)
            .IsUnique()
            .HasDatabaseName("idx_product_variants_sku");

        builder.OwnsOne(c => c.Price, price =>
        {
            price.Property(e => e.Amount)
                .HasColumnName("price")
                .HasColumnType("DECIMAL(10, 2)")
                .HasDefaultValue(0.0);
            price.Property(e => e.CurrencyType)
                .HasColumnName("price_currency")
                .HasMaxLength(5)
                .IsRequired();

            price.HasIndex(e => e.Amount)
                .HasDatabaseName("idx_product_variants_price_amount");

        });
        builder.OwnsOne(c => c.CompareAtPrice, price =>
        {
            price.Property(e => e.Amount)
                .HasColumnName("compare_at_price")
                .HasColumnType("DECIMAL(10, 2)")
                .HasDefaultValue(0.0);
            price.Property(e => e.CurrencyType)
                .HasColumnName("compare_at_price_currency")
                .HasMaxLength(5)
                .IsRequired(false);
        });

        builder.Property(pv => pv.VariantAttributes)
            .HasColumnName("variant_attributes")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(pv => pv.CustomizationOptions)
            .HasColumnName("customization_options")
            .HasColumnType("jsonb")
            .IsRequired();


        builder.Property(pv => pv.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(p => p.HasCheckConstraint(
            "chk_products_price",
            "price >= 0"
            ));
        
        builder.ToTable(p => p.HasCheckConstraint(
            "chk_products_compare_at_price",
            "compare_at_price IS NULL OR compare_at_price >= 0"
            ));
    }
}
