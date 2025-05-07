using System.ComponentModel.DataAnnotations;

namespace PorcupineUserManagement.Models;

public class Permission : BaseDbEntity
{
    [MaxLength(300)]
    public required string Name { get; set; }
}