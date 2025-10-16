using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Raporlama.Queries.GetLoanUsageStatistics;

public class LoanUsageStatisticsDto : IDto
{
    public Guid KutuphaneId { get; set; }
    public Guid MateryalId { get; set; }
    public string? MateryalBaslik { get; set; }
    public int ToplamOdunc { get; set; }
    public int AktifOdunc { get; set; }
    public int GecikenOdunc { get; set; }
    public int IadeEdilenOdunc { get; set; }
}
