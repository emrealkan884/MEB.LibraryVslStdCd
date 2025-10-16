using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Raporlama.Queries.GetLoanUsageStatistics;

public class GetLoanUsageStatisticsQuery : IRequest<List<LoanUsageStatisticsDto>>
{
    public Guid? KutuphaneId { get; set; }
    public DateTime? BaslangicTarihi { get; set; }
    public DateTime? BitisTarihi { get; set; }

    public class GetLoanUsageStatisticsQueryHandler : IRequestHandler<GetLoanUsageStatisticsQuery, List<LoanUsageStatisticsDto>>
    {
        private readonly IOduncIslemiRepository _oduncIslemiRepository;

        public GetLoanUsageStatisticsQueryHandler(IOduncIslemiRepository oduncIslemiRepository)
        {
            _oduncIslemiRepository = oduncIslemiRepository;
        }

        public async Task<List<LoanUsageStatisticsDto>> Handle(GetLoanUsageStatisticsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<OduncIslemi> query = _oduncIslemiRepository
                .Query()
                .Include(o => o.Nusha!)
                .ThenInclude(n => n.Materyal!)
                .ThenInclude(m => m.KatalogKaydi);

            if (request.KutuphaneId.HasValue)
                query = query.Where(o => o.KutuphaneId == request.KutuphaneId);

            if (request.BaslangicTarihi.HasValue)
                query = query.Where(o => o.AlinmaTarihi >= request.BaslangicTarihi.Value);

            if (request.BitisTarihi.HasValue)
                query = query.Where(o => o.AlinmaTarihi <= request.BitisTarihi.Value);

            DateTime utcNow = DateTime.UtcNow;

            List<LoanUsageStatisticsDto> usageStatistics = await query
                .Where(o => o.Nusha != null && o.Nusha.Materyal != null)
                .GroupBy(o => new
                {
                    o.KutuphaneId,
                    MateryalId = o.Nusha!.MateryalId,
                    MateryalBaslik = o.Nusha!.Materyal!.KatalogKaydi != null
                        ? o.Nusha.Materyal.KatalogKaydi.Baslik ?? o.Nusha.Materyal.KatalogKaydi.AltBaslik
                        : null
                })
                .Select(g => new LoanUsageStatisticsDto
                {
                    KutuphaneId = g.Key.KutuphaneId,
                    MateryalId = g.Key.MateryalId,
                    MateryalBaslik = g.Key.MateryalBaslik,
                    ToplamOdunc = g.Count(),
                    AktifOdunc = g.Count(x => x.Durumu == OduncDurumu.Aktif),
                    GecikenOdunc = g.Count(x =>
                        x.Durumu != OduncDurumu.IadeEdildi
                        && x.SonTeslimTarihi < utcNow),
                    IadeEdilenOdunc = g.Count(x => x.Durumu == OduncDurumu.IadeEdildi)
                })
                .OrderByDescending(x => x.ToplamOdunc)
                .ToListAsync(cancellationToken);

            return usageStatistics;
        }
    }
}
