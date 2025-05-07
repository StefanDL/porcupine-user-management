namespace PorcupineUserManagement.Models;

public class User : BaseDbEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}