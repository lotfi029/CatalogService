namespace Domain.Entities;

public class Product : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    public int Quentity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public ICollection<WishList> WishLists { get; set; } = [];
    public ICollection<Favourite> Favourites { get; set; } = [];
    public ICollection<BuyingHistory> BuyingHistories { get; set;} = [];
    public ICollection<UserBehavior> UserBehaviors { get; set; } = [];
    public ICollection<OrderItem> Orders { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    
}
