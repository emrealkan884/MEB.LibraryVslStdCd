namespace Domain.Entities.Security;

public class UserOperationClaim : NArchitecture.Core.Security.Entities.UserOperationClaim<Guid, Guid, int>
{
    public virtual User User { get; set; } = default!;
    public virtual OperationClaim OperationClaim { get; set; } = default!;
}
