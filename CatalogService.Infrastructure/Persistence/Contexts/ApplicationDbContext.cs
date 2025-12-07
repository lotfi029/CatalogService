using CatalogService.Domain.Abstractions;

namespace CatalogService.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryVariantAttribute> CategoryVariantAttributes { get; set; }
    public DbSet<ProductCategories> ProductCategories { get; set; }
    public DbSet<Domain.Entities.Attribute> Attributes {  get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<IAuditable>();

        foreach (var entityTrack in entities)
        {
            switch (entityTrack.State)
            {
                case EntityState.Added:
                    entityTrack.Entity.SetCreationAudit(Guid.NewGuid().ToString());
                    break;

                case EntityState.Modified:
                    if (entityTrack.Entity.IsDeleted)
                    {
                        var isDeletedProp = entityTrack.Property(e => e.IsDeleted);

                        if (isDeletedProp.IsModified && isDeletedProp.OriginalValue == false)
                        {
                            entityTrack.Entity.SetDeletionAudit(Guid.NewGuid().ToString());
                        }
                    }
                    else 
                        entityTrack.Entity.SetUpdateAudit(Guid.NewGuid().ToString());
                    break;
                default:
                    break;
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}