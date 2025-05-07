namespace PorcupineUserManagement.Models;

public abstract class BaseDbEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
    public bool IsDeleted { get; set; }
    
}