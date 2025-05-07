namespace PorcupineUserManagement.Models;

public class GroupUser : BaseDbEntity
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
}