using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new(){
                Id = DefaultRoles.Admin.Id,
                Name = DefaultRoles.Admin.Name,
                NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
            },
            new(){
                Id = DefaultRoles.NormalUser.Id,
                Name = DefaultRoles.NormalUser.Name,
                NormalizedName = DefaultRoles.NormalUser.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.NormalUser.ConcurrencyStamp
            }

        ]);
    }
}
