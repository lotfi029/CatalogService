using CatalogService.Domain.JsonProperties;
using System.Text.Json;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(pv => pv.Id)
            .HasColumnName("id");

        builder.ToTable("product_variants");

        builder.Property(pv => pv.SKU)
            .HasConversion(
                sku => sku.Value,
                value => new Sku(value)!
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
            .HasConversion(
                pv => JsonSerializer.Serialize(pv, (JsonSerializerOptions)null!),
                pv => JsonSerializer.Deserialize<ProductVariantsOption>(pv)!)
            .IsRequired();
        
        builder.Property(pv => pv.CustomizationOptions)
            .HasColumnName("customization_options")
            .HasColumnType("jsonb")
            .HasConversion(
                pv => JsonSerializer.Serialize(pv, (JsonSerializerOptions)null!),
                pv => JsonSerializer.Deserialize<ProductVariantsOption>(pv))
            .IsRequired(false);


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
