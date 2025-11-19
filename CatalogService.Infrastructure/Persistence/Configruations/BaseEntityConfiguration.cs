using CatalogService.Domain.Abstractions;

namespace CatalogService.Infrastructure.Persistence.Configruations;

internal class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450);
        
        builder.Property(e => e.LastUpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired(false);
        builder.Property(e => e.LastUpdatedBy)
            .HasColumnName("last_updated_by")
            .IsRequired(false);

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);
        builder.Property(e => e.DeletedBy)
            .HasColumnName("deleted_by")
            .IsRequired(false);
        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(b => b.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("idx_is_active");
    }
}