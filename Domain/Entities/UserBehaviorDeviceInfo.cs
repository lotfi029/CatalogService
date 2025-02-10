namespace Domain.Entities;

public class UserBehaviorDeviceInfo
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserBehaviorId { get; set; }
    public string OperatingSystem {  get; set; } = string.Empty;
    public string Browser {  get; set; } = string.Empty;
    public UserBehavior UserBehavior { get; set; } = default!;
}
