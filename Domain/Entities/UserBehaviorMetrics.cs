namespace Domain.Entities;

public class UserBehaviorMetrics
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserBehaviorId { get; set; }
    public int BounceRate { get; set; }
    public int ExitRate { get; set; }
    public int PageValue { get; set; }
    public int CTR { get; set; }
    public UserBehavior UserBehavior { get; set; } = default!;
}