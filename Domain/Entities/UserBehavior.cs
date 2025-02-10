namespace Domain.Entities;

public class UserBehavior : AuditableEntity
{
    public Guid ProductId {  get; set; }
    public int Day { get; set; }
    public Month Month { get; set; }
    public bool Weekend { get; set; }
    public bool Revenue {  get; set; }
    public string SearchKeyWord { get; set; } = string.Empty;
    public Product Product { get; set; } = default!;
    public ICollection<UserBehaviorAction> Actions { get; set; } = [];
    public ICollection<UserBehaviorMetrics> Metrics { get; set; } = [];
    public ICollection<UserBehaviorDeviceInfo> Devices { get; set; } = [];
}
