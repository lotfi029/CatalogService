namespace Domain.Entities;

public class Favourite
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
