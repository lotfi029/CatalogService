using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UpdatedById);

        builder.Property(e => e.IsDisabled)
            .HasDefaultValue(0);

        builder.Property(e => e.Content)
            .HasMaxLength(1000);
    }
}
