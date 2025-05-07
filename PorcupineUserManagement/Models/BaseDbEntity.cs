using System.ComponentModel.DataAnnotations;

namespace PorcupineUserManagement.Models;

public abstract class BaseDbEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; set; } = false;
    
}