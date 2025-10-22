using Application.Features.Raporlama.Queries.Common;
using System.Collections.Generic;

namespace Application.Features.Raporlama.Queries.GetUserSummary;

public class UserSummaryDto
{
    public IList<SummaryMetricDto> Metrics { get; set; } = new List<SummaryMetricDto>();
    public IList<RoleCountDto> RoleBreakdown { get; set; } = new List<RoleCountDto>();
}
