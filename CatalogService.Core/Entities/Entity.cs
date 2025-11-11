namespace CatalogService.Core.Entities;

public class Entity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt {  get; set; }
    public bool IsActive { get; set; } = true;
}
