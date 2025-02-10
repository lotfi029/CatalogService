using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;
public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permissions = Permissions.GetPermissions.ToList();
        var roleAdminClaim = new List<IdentityRoleClaim<string>>();
        var roleAdminId = DefaultRoles.Admin.Id;
        int cnt = 0;

        permissions.ForEach(e => roleAdminClaim.Add(
            new() 
            { 
                Id = ++cnt, 
                ClaimType = Permissions.Type, 
                ClaimValue = e, 
                RoleId = roleAdminId 
            })
        );

        builder.HasData(roleAdminClaim);
    }
}
