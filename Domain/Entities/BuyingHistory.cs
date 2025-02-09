namespace Domain.Entities;

public class BuyingHistory
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ProductId { get; set; }
    public int Quentity {  get; set; }
    public float Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Product Product { get; set; } = default!;
}

