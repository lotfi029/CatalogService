namespace Domain.Entities;
public class Order : AuditableEntity
{
    public float TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Default;
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public Product Product { get; set; } = default!;
}
