using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationRole : IdentityRole
{
    public bool IsDisabled { get; set; }
}
