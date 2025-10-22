using Application.Features.Raporlama.Queries.Common;
using Application.Services.Repositories;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.Raporlama.Queries.GetCatalogSummary;

public class GetCatalogSummaryQuery : IRequest<CatalogSummaryDto>
{
    /// <summary>
    /// Dahil edilecek metrik anahtarları. Null veya boş ise tüm metrikler döner.
    /// Kabul edilen anahtarlar: totalCatalogRecords, totalMaterials, totalCopies, availableCopies,
    /// loanedCopies, reservedCopies, maintenanceCopies, lostCopies.
    /// </summary>
    public IList<string>? Metrics { get; set; }

    public class GetCatalogSummaryQueryHandler : IRequestHandler<GetCatalogSummaryQuery, CatalogSummaryDto>
    {
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly IMateryalRepository _materyalRepository;
        private readonly INushaRepository _nushaRepository;

        public GetCatalogSummaryQueryHandler(
            IKatalogKaydiRepository katalogKaydiRepository,
            IMateryalRepository materyalRepository,
            INushaRepository nushaRepository
        )
        {
            _katalogKaydiRepository = katalogKaydiRepository;
            _materyalRepository = materyalRepository;
            _nushaRepository = nushaRepository;
        }

        public async Task<CatalogSummaryDto> Handle(GetCatalogSummaryQuery request, CancellationToken cancellationToken)
        {
            int totalCatalogRecords = await _katalogKaydiRepository.Query().CountAsync(cancellationToken);
            int totalMaterials = await _materyalRepository.Query().CountAsync(cancellationToken);

            IQueryable<Domain.Entities.Nusha> nushaQuery = _nushaRepository.Query();

            int totalCopies = await nushaQuery.CountAsync(cancellationToken);
            int availableCopies = await nushaQuery.Where(n => n.Durumu == NushaDurumu.Rafta).CountAsync(cancellationToken);
            int loanedCopies = await nushaQuery.Where(n => n.Durumu == NushaDurumu.Oduncta).CountAsync(cancellationToken);
            int reservedCopies = await nushaQuery.Where(n => n.Durumu == NushaDurumu.Ayirtildi).CountAsync(cancellationToken);
            int maintenanceCopies = await nushaQuery.Where(n => n.Durumu == NushaDurumu.Bakimda).CountAsync(cancellationToken);
            int lostCopies = await nushaQuery.Where(n => n.Durumu == NushaDurumu.Kayip).CountAsync(cancellationToken);

            Dictionary<string, SummaryMetricDto> metricMap = new()
            {
                ["totalCatalogRecords"] = new SummaryMetricDto("totalCatalogRecords", "Toplam Katalog Kaydı", totalCatalogRecords),
                ["totalMaterials"] = new SummaryMetricDto("totalMaterials", "Toplam Materyal", totalMaterials),
                ["totalCopies"] = new SummaryMetricDto("totalCopies", "Toplam Nüsha", totalCopies),
                ["availableCopies"] = new SummaryMetricDto("availableCopies", "Raftaki Nüsha", availableCopies),
                ["loanedCopies"] = new SummaryMetricDto("loanedCopies", "Ödünç Verilen Nüsha", loanedCopies),
                ["reservedCopies"] = new SummaryMetricDto("reservedCopies", "Ayırtılan Nüsha", reservedCopies),
                ["maintenanceCopies"] = new SummaryMetricDto("maintenanceCopies", "Bakımda Nüsha", maintenanceCopies),
                ["lostCopies"] = new SummaryMetricDto("lostCopies", "Kayıp Nüsha", lostCopies)
            };

            IEnumerable<string> selectedKeys = request.Metrics is { Count: > 0 }
                ? request.Metrics.Where(metricMap.ContainsKey)
                : metricMap.Keys;

            CatalogSummaryDto dto = new();
            foreach (string key in selectedKeys)
                dto.Metrics.Add(metricMap[key]);

            return dto;
        }
    }
}
