using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UpdatedById);

        builder.Property(e => e.IsDisabled)
            .HasDefaultValue(0);
    }
}
