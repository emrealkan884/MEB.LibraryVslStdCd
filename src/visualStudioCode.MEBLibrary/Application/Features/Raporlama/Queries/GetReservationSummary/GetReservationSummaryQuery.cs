using Application.Features.Raporlama.Queries.Common;
using Application.Services.Repositories;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.Raporlama.Queries.GetReservationSummary;

public class GetReservationSummaryQuery : IRequest<ReservationSummaryDto>
{
    /// <summary>
    /// Dahil edilecek metrik anahtarları. Null/boş ise tümü. Anahtarlar:
    /// totalReservations, pendingReservations, readyReservations, completedReservations, cancelledReservations, expiredReservations.
    /// </summary>
    public IList<string>? Metrics { get; set; }

    public class GetReservationSummaryQueryHandler : IRequestHandler<GetReservationSummaryQuery, ReservationSummaryDto>
    {
        private readonly IRezervasyonRepository _rezervasyonRepository;

        public GetReservationSummaryQueryHandler(IRezervasyonRepository rezervasyonRepository)
        {
            _rezervasyonRepository = rezervasyonRepository;
        }

        public async Task<ReservationSummaryDto> Handle(GetReservationSummaryQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Rezervasyon> query = _rezervasyonRepository.Query();

            int total = await query.CountAsync(cancellationToken);
            int pending = await query.Where(r => r.Durumu == RezervasyonDurumu.Beklemede).CountAsync(cancellationToken);
            int ready = await query.Where(r => r.Durumu == RezervasyonDurumu.Hazir).CountAsync(cancellationToken);
            int completed = await query.Where(r => r.Durumu == RezervasyonDurumu.Tamamlandi).CountAsync(cancellationToken);
            int cancelled = await query.Where(r => r.Durumu == RezervasyonDurumu.Iptal).CountAsync(cancellationToken);
            int expired = await query.Where(r => r.Durumu == RezervasyonDurumu.SureDoldu).CountAsync(cancellationToken);

            Dictionary<string, SummaryMetricDto> metricMap = new()
            {
                ["totalReservations"] = new SummaryMetricDto("totalReservations", "Toplam Rezervasyon", total),
                ["pendingReservations"] = new SummaryMetricDto("pendingReservations", "Beklemede Rezervasyon", pending),
                ["readyReservations"] = new SummaryMetricDto("readyReservations", "Hazır Rezervasyon", ready),
                ["completedReservations"] = new SummaryMetricDto("completedReservations", "Tamamlanan Rezervasyon", completed),
                ["cancelledReservations"] = new SummaryMetricDto("cancelledReservations", "İptal Edilen Rezervasyon", cancelled),
                ["expiredReservations"] = new SummaryMetricDto("expiredReservations", "Süresi Dolan Rezervasyon", expired)
            };

            IEnumerable<string> selectedKeys = request.Metrics is { Count: > 0 }
                ? request.Metrics.Where(metricMap.ContainsKey)
                : metricMap.Keys;

            ReservationSummaryDto dto = new();
            foreach (string key in selectedKeys)
                dto.Metrics.Add(metricMap[key]);

            return dto;
        }
    }
}
