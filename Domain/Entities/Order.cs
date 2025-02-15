namespace Domain.Entities;
public class Order : AuditableEntity
{
    public Guid TraderId { get; set; }
    public int TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Default;
    public ICollection<OrderItem> Items { get; set; } = [];
}
