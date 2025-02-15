namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrderId { get; set; }
    public int Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Default;
    public DateTime PaymentDate { get; set; }
    public Order Order { get; set; } = default!;
}
