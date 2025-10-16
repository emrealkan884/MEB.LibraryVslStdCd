using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Raporlama.Queries.GetOverdueLoans;

public class GetOverdueLoansReportQuery : IRequest<List<OverdueLoanReportDto>>
{
    public Guid? KutuphaneId { get; set; }
    public Guid? KullaniciId { get; set; }

    public class GetOverdueLoansReportQueryHandler : IRequestHandler<GetOverdueLoansReportQuery, List<OverdueLoanReportDto>>
    {
        private readonly IOduncIslemiRepository _oduncIslemiRepository;

        public GetOverdueLoansReportQueryHandler(IOduncIslemiRepository oduncIslemiRepository)
        {
            _oduncIslemiRepository = oduncIslemiRepository;
        }

        public async Task<List<OverdueLoanReportDto>> Handle(GetOverdueLoansReportQuery request, CancellationToken cancellationToken)
        {
            DateTime utcNow = DateTime.UtcNow;

            IQueryable<OduncIslemi> query = _oduncIslemiRepository
                .Query()
                .Include(o => o.Nusha!)
                .ThenInclude(n => n.Materyal!)
                .ThenInclude(m => m.KatalogKaydi)
                .Where(o =>
                    o.Durumu != OduncDurumu.IadeEdildi
                    && o.SonTeslimTarihi < utcNow);

            if (request.KutuphaneId.HasValue)
                query = query.Where(o => o.KutuphaneId == request.KutuphaneId);

            if (request.KullaniciId.HasValue)
                query = query.Where(o => o.KullaniciId == request.KullaniciId);

            List<OduncIslemi> overdueLoans = await query.ToListAsync(cancellationToken);

            List<OverdueLoanReportDto> reportItems =
                overdueLoans
                    .Select(o => new OverdueLoanReportDto
                    {
                        OduncIslemiId = o.Id,
                        KutuphaneId = o.KutuphaneId,
                        KullaniciId = o.KullaniciId,
                        NushaId = o.NushaId,
                        MateryalId = o.Nusha?.MateryalId,
                        MateryalBaslik = o.Nusha?.Materyal?.KatalogKaydi?.Baslik ?? o.Nusha?.Materyal?.KatalogKaydi?.AltBaslik,
                        AlinmaTarihi = o.AlinmaTarihi,
                        SonTeslimTarihi = o.SonTeslimTarihi,
                        GecikenGun = (int)Math.Ceiling((utcNow - o.SonTeslimTarihi).TotalDays)
                    })
                    .OrderByDescending(x => x.GecikenGun)
                    .ToList();

            return reportItems;
        }
    }
}
