using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>()
        {
            RoleId = DefaultRoles.Admin.Id,
            UserId = DefaultUsers.Admin.Id,
        });
    }
}
