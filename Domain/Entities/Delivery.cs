namespace Domain.Entities;

public class Delivery
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrderId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Default;
    public DateTime EstimatedTime { get; set; }
    public Order Order { get; set; } = default!;
}
