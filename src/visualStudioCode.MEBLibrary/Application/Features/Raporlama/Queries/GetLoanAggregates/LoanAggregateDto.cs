using System.ComponentModel.DataAnnotations;

namespace Application.Features.Raporlama.Queries.GetLoanAggregates;

public class LoanAggregateDto
{
    public LoanAggregateDimension Dimension { get; set; }
    public Guid? EntityId { get; set; }

    [Required]
    public string DisplayName { get; set; } = string.Empty;

    public int LoanCount { get; set; }
}
