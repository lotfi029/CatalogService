using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class DriverProfileConfiguration : IEntityTypeConfiguration<DriverProfile>
{
    public void Configure(EntityTypeBuilder<DriverProfile> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<DriverProfile>(e => e.Id);

        builder.Property(e => e.VehicleType)
            .HasMaxLength(100);

        builder.Property(e => e.VehicleLicense)
            .HasMaxLength(100);

        builder.Property(e => e.VehicleColor)
            .HasMaxLength(50);

        builder.Property(e => e.VehiclePlateNumber)
            .HasMaxLength(50);

        builder.Property(e => e.DrivingLicense)
            .HasMaxLength(100);


        builder.Property(e => e.WalletBalance)
            .HasDefaultValue(0);
        
        builder.Property(e => e.TotalDiscount)
            .HasDefaultValue(0);
        
        builder.Property(e => e.TotalTrips)
            .HasDefaultValue(0);

        builder.Property(e => e.TotalEarnings)
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .HasDefaultValue(1);

        builder.Property(e => e.Rating)
            .HasDefaultValue(0);

        builder.Property(e => e.IsVerified)
            .HasDefaultValue(0);
    }
}
