using System.ComponentModel.DataAnnotations;

namespace PorcupineUserManagement.Models;

public class User : BaseDbEntity
{
    [MaxLength(300)]
    public required string Name { get; set; }
    [MaxLength(300)]
    public required string Email { get; set; }
}