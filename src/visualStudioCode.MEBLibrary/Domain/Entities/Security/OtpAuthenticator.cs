namespace Domain.Entities.Security;

public class OtpAuthenticator : NArchitecture.Core.Security.Entities.OtpAuthenticator<Guid>
{
    public virtual User User { get; set; } = default!;
}
