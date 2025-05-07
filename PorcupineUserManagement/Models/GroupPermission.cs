namespace PorcupineUserManagement.Models;

public class GroupPermission : BaseDbEntity
{
    public Guid GroupId { get; set; }
    public Guid PermissionId { get; set; }
}