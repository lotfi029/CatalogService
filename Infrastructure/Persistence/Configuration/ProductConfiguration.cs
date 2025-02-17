using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UpdatedById);

        builder.Property(e => e.IsDisabled)
            .HasDefaultValue(0);
        
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(0);

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);


    }
}
