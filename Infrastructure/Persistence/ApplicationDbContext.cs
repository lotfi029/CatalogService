using Domain.Comman;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Claims;

namespace Infrastructure.Persistence;
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<WishList> WishList { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<UserBehavior> UserBehaviors { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(e => e.DeleteBehavior == DeleteBehavior.Cascade && !e.IsOwnership);

        foreach (var item in cascadeFKs)
            item.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entiries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        foreach (var entiry in entiries)
        {
            if (entiry.State == EntityState.Added)
            {
                entiry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                entiry.Property(e => e.CreatedById).CurrentValue = currentUserId;
            }
            else if (entiry.State == EntityState.Modified)
            {
                entiry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                entiry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
