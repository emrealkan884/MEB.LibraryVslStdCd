using Application.Features.Raporlama.Queries.Common;
using System.Collections.Generic;

namespace Application.Features.Raporlama.Queries.GetReservationSummary;

public class ReservationSummaryDto
{
    public IList<SummaryMetricDto> Metrics { get; set; } = new List<SummaryMetricDto>();
}
