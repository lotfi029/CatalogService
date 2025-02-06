using Infrastructure.Abstractions.Constants;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(e => e.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(e => e.FirstName)
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .HasMaxLength(100);

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.Id,
            FirstName = DefaultUsers.FirstName,
            LastName = DefaultUsers.LastName,
            UserName = DefaultUsers.UserName,
            NormalizedUserName = DefaultUsers.UserName.ToUpper(),
            Email = DefaultUsers.Email,
            NormalizedEmail = DefaultUsers.Email.ToUpper(),
            EmailConfirmed = true,
            Region = DefaultUsers.Region,
            VisitorType = DefaultUsers.VisitorType,
            ConcurrencyStamp = DefaultUsers.ConcurrencyStamp,
            SecurityStamp = DefaultUsers.SecurityStamp,
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2025, 1, 5)),
            PasswordHash = "AQAAAAIAAYagAAAAECl9JOvxdQxpuavAKUNQ3NekBoKCjmJP/JgztKEreCfMtOTrv/ZnKq5gFycMbZIbzA=="
        });
    }
}
