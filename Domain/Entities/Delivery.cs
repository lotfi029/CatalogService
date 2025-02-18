namespace Domain.Entities;

public class Delivery
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrderId { get; set; }
    public string DriverId { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime AssignedAt { get; set; }
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
    public DateTime EstimatedTime { get; set; }
    public Order Order { get; set; } = default!;
    public DriverProfile Driver { get; set; } = default!;
}
