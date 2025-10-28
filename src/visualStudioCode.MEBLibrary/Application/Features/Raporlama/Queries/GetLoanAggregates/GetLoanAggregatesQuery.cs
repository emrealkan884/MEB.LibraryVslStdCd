using System.Linq;
using Application.Authorization;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Raporlama.Queries.GetLoanAggregates;

public class GetLoanAggregatesQuery : IRequest<List<LoanAggregateDto>>
{
    [Display(Description = "Rapor boyutu: 1=Kitap, 2=Yazar, 3=Kutuphane.")]
    public LoanAggregateDimension Dimension { get; set; } = LoanAggregateDimension.Book;

    [Display(Description = "Dondurulecek kayit sayisi (varsayilan 10).")]
    public int? Top { get; set; }

    [Display(Description = "Belirli bir Kutuphane icin filtreleme yapmak icin kullanilir.")]
    public Guid? KutuphaneId { get; set; }

    [Display(Description = "Baslangic tarihi (UTC).")]
    public DateTime? BaslangicTarihi { get; set; }

    [Display(Description = "Bitis tarihi (UTC).")]
    public DateTime? BitisTarihi { get; set; }

    public class GetLoanAggregatesQueryHandler : IRequestHandler<GetLoanAggregatesQuery, List<LoanAggregateDto>>
    {
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly IKullaniciYetkiServisi _kullaniciYetkiServisi;

        public GetLoanAggregatesQueryHandler(
            IOduncIslemiRepository oduncIslemiRepository,
            IKullaniciYetkiServisi kullaniciYetkiServisi
        )
        {
            _oduncIslemiRepository = oduncIslemiRepository;
            _kullaniciYetkiServisi = kullaniciYetkiServisi;
        }

        public async Task<List<LoanAggregateDto>> Handle(GetLoanAggregatesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Guid>? yetkiliKutuphaneler = await _kullaniciYetkiServisi.YetkiliKutuphaneIdListesiAsync(cancellationToken);
            if (yetkiliKutuphaneler is { Count: 0 })
                return new List<LoanAggregateDto>();

            IQueryable<Domain.Entities.OduncIslemi> baseQuery = _oduncIslemiRepository
                .Query()
                .AsNoTracking()
                .Include(o => o.Kutuphane)
                .Include(o => o.Nusha)
                    .ThenInclude(n => n.Materyal)
                        .ThenInclude(m => m.KatalogKaydi)
                            .ThenInclude(k => k.KatalogYazarlar)
                                .ThenInclude(ky => ky.Yazar);

            if (yetkiliKutuphaneler is not null)
            {
                HashSet<Guid> kutuphaneSet = yetkiliKutuphaneler.ToHashSet();
                baseQuery = baseQuery.Where(o => kutuphaneSet.Contains(o.KutuphaneId));
            }

            if (request.KutuphaneId.HasValue)
                baseQuery = baseQuery.Where(o => o.KutuphaneId == request.KutuphaneId);

            if (request.BaslangicTarihi.HasValue)
                baseQuery = baseQuery.Where(o => o.AlinmaTarihi >= request.BaslangicTarihi);

            if (request.BitisTarihi.HasValue)
                baseQuery = baseQuery.Where(o => o.AlinmaTarihi <= request.BitisTarihi);

            int top = request.Top.HasValue && request.Top.Value > 0 ? request.Top.Value : 10;

            return request.Dimension switch
            {
                LoanAggregateDimension.Author => await buildAuthorAggregates(baseQuery, top, cancellationToken),
                LoanAggregateDimension.Library => await buildLibraryAggregates(baseQuery, top, cancellationToken),
                _ => await buildBookAggregates(baseQuery, top, cancellationToken)
            };
        }

        private static async Task<List<LoanAggregateDto>> buildBookAggregates(
            IQueryable<Domain.Entities.OduncIslemi> query,
            int top,
            CancellationToken cancellationToken
        )
        {
            return await query
                .Where(o => o.Nusha != null && o.Nusha.Materyal != null && o.Nusha.Materyal.KatalogKaydi != null)
                .GroupBy(o => new
                {
                    o.Nusha!.Materyal!.KatalogKaydiId,
                    Title = o.Nusha!.Materyal!.KatalogKaydi!.Baslik
                })
                .Select(g => new LoanAggregateDto
                {
                    Dimension = LoanAggregateDimension.Book,
                    EntityId = g.Key.KatalogKaydiId,
                    DisplayName = g.Key.Title ?? "Belirtilmemis",
                    LoanCount = g.Count()
                })
                .OrderByDescending(x => x.LoanCount)
                .ThenBy(x => x.DisplayName)
                .Take(top)
                .ToListAsync(cancellationToken);
        }

        private static async Task<List<LoanAggregateDto>> buildAuthorAggregates(
            IQueryable<Domain.Entities.OduncIslemi> query,
            int top,
            CancellationToken cancellationToken
        )
        {
            return await query
                .Where(o => o.Nusha != null && o.Nusha.Materyal != null && o.Nusha.Materyal.KatalogKaydi != null)
                .SelectMany(o => o.Nusha!.Materyal!.KatalogKaydi!.KatalogYazarlar
                    .Select(ky => new
                    {
                        ky.YazarId,
                        AuthorName = ky.Yazar != null && ky.Yazar.AdSoyad != null
                            ? ky.Yazar.AdSoyad
                            : "Belirtilmemis"
                    }))
                .GroupBy(x => new { x.YazarId, x.AuthorName })
                .Select(g => new LoanAggregateDto
                {
                    Dimension = LoanAggregateDimension.Author,
                    EntityId = g.Key.YazarId,
                    DisplayName = g.Key.AuthorName,
                    LoanCount = g.Count()
                })
                .OrderByDescending(x => x.LoanCount)
                .ThenBy(x => x.DisplayName)
                .Take(top)
                .ToListAsync(cancellationToken);
        }

        private static async Task<List<LoanAggregateDto>> buildLibraryAggregates(
            IQueryable<Domain.Entities.OduncIslemi> query,
            int top,
            CancellationToken cancellationToken
        )
        {
            return await query
                .Where(o => o.Kutuphane != null)
                .GroupBy(o => new
                {
                    o.KutuphaneId,
                    LibraryName = o.Kutuphane != null && o.Kutuphane.Ad != null ? o.Kutuphane.Ad : "Belirtilmemis"
                })
                .Select(g => new LoanAggregateDto
                {
                    Dimension = LoanAggregateDimension.Library,
                    EntityId = g.Key.KutuphaneId,
                    DisplayName = g.Key.LibraryName,
                    LoanCount = g.Count()
                })
                .OrderByDescending(x => x.LoanCount)
                .ThenBy(x => x.DisplayName)
                .Take(top)
                .ToListAsync(cancellationToken);
        }
    }
}




