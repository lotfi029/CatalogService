namespace Domain.Entities;

public class UserBehaviorAction
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserBehaviorId { get; set; }
    public Guid ActionId { get; set; }
    public TimeSpan Duration {  get; set; }
    public int ActionCount { get; set; }
    public UserBehavior UserBehavior { get; set; } = default!;
    public ActionType Action {  get; set; } = default!;
}
