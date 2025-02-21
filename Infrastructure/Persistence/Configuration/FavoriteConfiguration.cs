using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder.HasOne(e => e.Product)
            .WithMany(e => e.Favourites)
            .HasForeignKey(r => r.ProductId);
    }
}
