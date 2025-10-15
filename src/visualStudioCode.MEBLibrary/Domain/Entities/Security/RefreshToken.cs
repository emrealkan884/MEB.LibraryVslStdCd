namespace Domain.Entities.Security;

public class RefreshToken : NArchitecture.Core.Security.Entities.RefreshToken<Guid, Guid>
{
    public virtual User User { get; set; } = default!;
}
