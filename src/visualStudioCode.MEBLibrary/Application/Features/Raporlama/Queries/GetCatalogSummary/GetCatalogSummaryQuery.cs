using System.Collections.Generic;
using System.Linq;
using Application.Authorization;
using Application.Features.Raporlama.Queries.Common;
using Application.Services.Repositories;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IKullaniciYetkiServisi _kullaniciYetkiServisi;

        public GetCatalogSummaryQueryHandler(
            IKatalogKaydiRepository katalogKaydiRepository,
            IMateryalRepository materyalRepository,
            INushaRepository nushaRepository,
            IKullaniciYetkiServisi kullaniciYetkiServisi
        )
        {
            _katalogKaydiRepository = katalogKaydiRepository;
            _materyalRepository = materyalRepository;
            _nushaRepository = nushaRepository;
            _kullaniciYetkiServisi = kullaniciYetkiServisi;
        }

        public async Task<CatalogSummaryDto> Handle(GetCatalogSummaryQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Guid>? yetkiliKutuphaneler =
                await _kullaniciYetkiServisi.YetkiliKutuphaneIdListesiAsync(cancellationToken);
            if (yetkiliKutuphaneler is { Count: 0 })
                return new CatalogSummaryDto();

            HashSet<Guid>? kutuphaneSet = yetkiliKutuphaneler?.ToHashSet();

            IQueryable<Domain.Entities.KatalogKaydi> katalogKaydiQuery = _katalogKaydiRepository.Query();
            if (kutuphaneSet is not null)
                katalogKaydiQuery = katalogKaydiQuery.Where(k => kutuphaneSet.Contains(k.KutuphaneId));

            int totalCatalogRecords = await katalogKaydiQuery.CountAsync(cancellationToken);

            IQueryable<Domain.Entities.Materyal> materyalQuery = _materyalRepository.Query();
            if (kutuphaneSet is not null)
                materyalQuery = materyalQuery.Where(m => kutuphaneSet.Contains(m.KutuphaneId));

            int totalMaterials = await materyalQuery.CountAsync(cancellationToken);

            IQueryable<Domain.Entities.Nusha> nushaQuery = _nushaRepository.Query();
            if (kutuphaneSet is not null)
                nushaQuery = nushaQuery.Where(n => n.Materyal != null && kutuphaneSet.Contains(n.Materyal.KutuphaneId));

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
