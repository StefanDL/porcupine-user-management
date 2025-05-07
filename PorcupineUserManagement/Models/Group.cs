using System.ComponentModel.DataAnnotations;

namespace PorcupineUserManagement.Models;

public class Group : BaseDbEntity
{
    [MaxLength(300)]
    public required string Name { get; set; }
}

