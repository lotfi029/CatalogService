using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class TraderProfileConfiguration : IEntityTypeConfiguration<TraderProfile>
{
    public void Configure(EntityTypeBuilder<TraderProfile> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<TraderProfile>(e => e.Id);

        builder.Property(e => e.CompanyName)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyAddress)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyPhone)
            .HasMaxLength(50);

        builder.Property(e => e.CompanyEmail)
            .HasMaxLength(50);

        builder.Property(e => e.CompanyWebsite)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyLogo)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyDescription)
            .HasMaxLength(1000);

        builder.Property(e => e.CompanyTaxNumber)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyType)
            .HasMaxLength(100);

        builder.Property(e => e.CompanyLicense)
            .HasMaxLength(100);

        builder.Property(e => e.WalletBalance)
            .HasDefaultValue(0);

        builder.Property(e => e.Rating)
            .HasDefaultValue(0);

        builder.Property(e => e.IsVerified)
            .HasDefaultValue(0);
    }
}