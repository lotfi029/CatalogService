using Domain.Entities;

namespace Domain.Comman;
public class AuditEntity 
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public bool IsDisabled { get; set; } = false;
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ApplicationUser CreatedBy { get; set; } = default!;
    public string? UpdatedById { get; set; } = string.Empty; 
    public DateTime? UpdatedAt { get; set; }
    public ApplicationUser? UpdatedBy { get; set; }
}
