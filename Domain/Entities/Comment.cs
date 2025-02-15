namespace Domain.Entities;
public class Comment : AuditableEntity
{
    public string Content { get; set; } = string.Empty;
    public Product Product { get; set; } = default!;
}
