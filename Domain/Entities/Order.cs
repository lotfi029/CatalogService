namespace Domain.Entities;
public class Order : AuditableEntity
{
    public int TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Default;
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public Product Product { get; set; } = default!;
}
