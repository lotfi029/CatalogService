using CatalogService.Domain.Abstractions;
using CatalogService.Infrastructure.DomainEvents;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CatalogService.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor,
    IDomainEventsDispatcher domainEventsDispatcher) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryVariantAttribute> CategoryVariantAttributes { get; set; }
    public DbSet<VariantAttributeDefinition> VariantAttributeDefinitions { get; set; }
    public DbSet<ProductCategories> ProductCategories { get; set; }
    public DbSet<Domain.Entities.Attribute> Attributes {  get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var entities = ChangeTracker.Entries<IAuditable>();

        foreach (var entityTrack in entities)
        {
            switch (entityTrack.State)
            {
                case EntityState.Added:
                    entityTrack.Entity.SetCreationAudit(userId);
                    break;

                case EntityState.Modified:
                    if (entityTrack.Entity.IsDeleted)
                    {
                        var isDeletedProp = entityTrack.Property(e => e.IsDeleted);

                        if (isDeletedProp.IsModified && isDeletedProp.OriginalValue == false)
                        {
                            entityTrack.Entity.SetDeletionAudit(userId);
                        }
                    }
                    else 
                        entityTrack.Entity.SetUpdateAudit(userId);
                    break;
                case EntityState.Deleted:
                    entityTrack.Entity.SetDeletionAudit(userId);
                    entityTrack.State = EntityState.Modified;
                    break;
                default:
                    break;
            }
        }

        int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await PublicDomainEventsAsync(cancellationToken);
        return result;
    }
    private async Task PublicDomainEventsAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvent = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();
                return domainEvent;
            }).ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents, ct);
    }
}