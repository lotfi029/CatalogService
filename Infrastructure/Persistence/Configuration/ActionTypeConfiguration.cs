using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Abstractions.Constants.DefaultActions;

namespace Infrastructure.Persistence.Configuration;
public class ActionTypeConfiguration : IEntityTypeConfiguration<ActionType>
{
    public void Configure(EntityTypeBuilder<ActionType> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        List<ActionType> actions = [
            new() {
                Id = Guid.Parse(Purchase.Id),
                Name = Purchase.Name,
                Description = Purchase.Description,
                CreatedAt = DateTime.Parse("02-10-2025"),
                CreatedById = DefaultUsers.Admin.Id,
            },
            new() {
                Id = Guid.Parse(AddToWishList.Id),
                Name = AddToWishList.Name,
                Description = AddToWishList.Description,
                CreatedAt = DateTime.Parse("02-10-2025"),
                CreatedById = DefaultUsers.Admin.Id,
            },
            new() {
                Id = Guid.Parse(AddToFavourites.Id),
                Name = AddToFavourites.Name,
                Description = AddToFavourites.Description,
                CreatedAt = DateTime.Parse("02-10-2025"),
                CreatedById = DefaultUsers.Admin.Id
            },
            new() {
                Id = Guid.Parse(View.Id),
                Name = View.Name,
                Description = View.Description,
                CreatedAt = DateTime.Parse("02-10-2025"),
                CreatedById = DefaultUsers.Admin.Id
            }
            ];

        builder.HasData(actions);
    }
}
