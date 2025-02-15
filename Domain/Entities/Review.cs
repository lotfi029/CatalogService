namespace Domain.Entities;

public class Review : AuditableEntity
{
    public int Rate { get; set; }
    public Product Product { get; set; } = default!;
}