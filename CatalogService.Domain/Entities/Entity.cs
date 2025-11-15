namespace CatalogService.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt {  get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

public abstract class AggregateRoot : Entity
{
    
}