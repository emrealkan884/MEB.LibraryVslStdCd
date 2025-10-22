using Application.Features.Raporlama.Queries.Common;
using System.Collections.Generic;

namespace Application.Features.Raporlama.Queries.GetCatalogSummary;

public class CatalogSummaryDto
{
    public IList<SummaryMetricDto> Metrics { get; set; } = new List<SummaryMetricDto>();
}
