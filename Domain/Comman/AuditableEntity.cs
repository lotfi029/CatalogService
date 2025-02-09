using Domain.Entities;

namespace Domain.Comman;
public class AuditableEntity 
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public bool IsDisabled { get; set; } = false;
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedById { get; set; } = string.Empty; 
    public DateTime? UpdatedAt { get; set; }
}
