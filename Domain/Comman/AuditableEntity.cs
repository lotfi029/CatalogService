

namespace Domain.Comman;
public class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedById { get; set; } = string.Empty;
    public DateTime? ModifiedAt { get; set; }
}
