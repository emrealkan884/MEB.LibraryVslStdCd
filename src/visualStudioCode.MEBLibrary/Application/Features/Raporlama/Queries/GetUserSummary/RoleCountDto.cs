namespace Application.Features.Raporlama.Queries.GetUserSummary;

public class RoleCountDto
{
    public string RoleKey { get; set; } = string.Empty;
    public string RoleLabel { get; set; } = string.Empty;
    public int UserCount { get; set; }
}
