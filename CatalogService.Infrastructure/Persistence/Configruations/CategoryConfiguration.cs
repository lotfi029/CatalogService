namespace CatalogService.Infrastructure.Persistence.Configruations;

internal class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("categories");
        
        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .IsRequired(false);


        builder.Property(c => c.ParentId)
            .HasColumnName("parent_id")
            .IsRequired(false);

        builder.HasOne(c => c.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.Property(c => c.Slug)
            .HasColumnName("slug")
            .HasMaxLength(200)
            .IsRequired();
        builder.HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("idx_categories_slug");

        builder.Property(c => c.Path)
            .HasColumnName("path")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(c => c.Level)
            .HasColumnName("level")
            .IsRequired();
        builder.HasIndex(c => c.Level)
            .IsUnique()
            .HasDatabaseName("idx_categories_level");

        builder.Property(c => c.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.ToTable(c => c.HasCheckConstraint(
            "chk_categories_level",
            "level >= 0"
            ));
        // check constraint to ensure level is non-negative
    }
}
