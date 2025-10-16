using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class AuditLog : Entity<Guid>
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string ActionName { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public string? ClientIp { get; set; }
    public string? UserAgent { get; set; }
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}
