namespace Domain.Entities.Security;

public class User : NArchitecture.Core.Security.Entities.User<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public string? SorumluIlKodu { get; set; }
    public string? SorumluIlceKodu { get; set; }
    public Guid? SorumluKutuphaneId { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<OtpAuthenticator> OtpAuthenticators { get; set; } = default!;
    public virtual ICollection<EmailAuthenticator> EmailAuthenticators { get; set; } = default!;
}
