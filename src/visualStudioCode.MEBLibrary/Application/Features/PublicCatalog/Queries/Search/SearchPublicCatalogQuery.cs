using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PublicCatalog.Queries.Search;

public class SearchPublicCatalogQuery : IRequest<PublicCatalogSearchResponse>
{
    public string? Query { get; set; }
    public string? MaterialType { get; set; }
    public string? Language { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public class SearchPublicCatalogQueryHandler : IRequestHandler<SearchPublicCatalogQuery, PublicCatalogSearchResponse>
    {
        private static readonly Dictionary<string, MateryalTuru[]> MaterialTypeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["book"] = new[] { MateryalTuru.Kitap, MateryalTuru.EKitap },
            ["magazine"] = new[] { MateryalTuru.SureliYayin },
            ["media"] = new[] { MateryalTuru.Multimedya, MateryalTuru.GorselMateryal },
            ["thesis"] = new[] { MateryalTuru.Kitap, MateryalTuru.Diger }
        };

        private readonly IKatalogKaydiRepository _katalogKaydiRepository;

        public SearchPublicCatalogQueryHandler(IKatalogKaydiRepository katalogKaydiRepository)
        {
            _katalogKaydiRepository = katalogKaydiRepository;
        }

        public async Task<PublicCatalogSearchResponse> Handle(SearchPublicCatalogQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page <= 0 ? 1 : request.Page;
            int size = request.PageSize <= 0 ? 12 : Math.Min(request.PageSize, 48);

            if (request.YearFrom.HasValue && request.YearTo.HasValue && request.YearFrom > request.YearTo)
            {
                (request.YearFrom, request.YearTo) = (request.YearTo, request.YearFrom);
            }

            IQueryable<KatalogKaydi> queryable = _katalogKaydiRepository.Query().AsNoTracking();

            queryable = ApplyFilters(queryable, request);
            queryable = ApplySorting(queryable, request.Sort);

            int totalCount = await queryable.CountAsync(cancellationToken);

            List<KatalogKaydi> records = await queryable
                .Include(k => k.KatalogYazarlar)
                    .ThenInclude(ky => ky.Yazar)
                .Include(k => k.Materyaller)
                    .ThenInclude(m => m.Kutuphane)
                .Include(k => k.Materyaller)
                    .ThenInclude(m => m.Nushalar)
                .Include(k => k.Kutuphane)
                .AsSplitQuery()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            IReadOnlyCollection<PublicCatalogRecordDto> items = records
                .Select(MapToDto)
                .ToArray();

            return new PublicCatalogSearchResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = size
            };
        }

        private static IQueryable<KatalogKaydi> ApplyFilters(IQueryable<KatalogKaydi> queryable, SearchPublicCatalogQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                string search = request.Query.Trim();
                string likeExpression = $"%{search}%";

                queryable = queryable.Where(katalog =>
                    (katalog.Baslik != null && EF.Functions.Like(katalog.Baslik, likeExpression)) ||
                    (katalog.AltBaslik != null && EF.Functions.Like(katalog.AltBaslik, likeExpression)) ||
                    (katalog.Isbn != null && EF.Functions.Like(katalog.Isbn, likeExpression)) ||
                    katalog.KatalogYazarlar.Any(yazar =>
                        yazar.Yazar != null &&
                        yazar.Yazar.AdSoyad != null &&
                        EF.Functions.Like(yazar.Yazar.AdSoyad, likeExpression)));
            }

            if (!string.IsNullOrWhiteSpace(request.MaterialType) && !string.Equals(request.MaterialType, "all", StringComparison.OrdinalIgnoreCase))
            {
                if (MaterialTypeMap.TryGetValue(request.MaterialType.Trim(), out MateryalTuru[]? mappedTypes))
                {
                    queryable = queryable.Where(katalog => mappedTypes.Contains(katalog.MateryalTuru));
                }
                else
                {
                    string loweredMaterial = request.MaterialType.Trim().ToLower();
                    queryable = queryable.Where(katalog =>
                        katalog.MateryalTuru.ToString().ToLower().Contains(loweredMaterial) ||
                        (katalog.MateryalAltTuru != null && katalog.MateryalAltTuru.ToLower().Contains(loweredMaterial)));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Language) && !string.Equals(request.Language, "all", StringComparison.OrdinalIgnoreCase))
            {
                string loweredLang = request.Language.Trim().ToLower();
                queryable = queryable.Where(katalog => katalog.Dil != null && katalog.Dil.ToLower().Contains(loweredLang));
            }

            if (request.YearFrom.HasValue)
            {
                queryable = queryable.Where(katalog => katalog.YayinYili.HasValue && katalog.YayinYili.Value >= request.YearFrom.Value);
            }

            if (request.YearTo.HasValue)
            {
                queryable = queryable.Where(katalog => katalog.YayinYili.HasValue && katalog.YayinYili.Value <= request.YearTo.Value);
            }

            return queryable;
        }

        private static IQueryable<KatalogKaydi> ApplySorting(IQueryable<KatalogKaydi> queryable, string? sort)
        {
            return sort?.ToLowerInvariant() switch
            {
                "recent" => queryable.OrderByDescending(k => k.KayitTarihi),
                "year-desc" => queryable.OrderByDescending(k => k.YayinYili).ThenBy(k => k.Baslik),
                "year-asc" => queryable.OrderBy(k => k.YayinYili).ThenBy(k => k.Baslik),
                "title" => queryable.OrderBy(k => k.Baslik),
                _ => queryable.OrderByDescending(k => k.KayitTarihi)
            };
        }

        private static PublicCatalogRecordDto MapToDto(KatalogKaydi katalogKaydi)
        {
            string author = katalogKaydi.KatalogYazarlar
                ?.OrderBy(y => y.Sira)
                .Select(y => y.Yazar?.AdSoyad?.Trim())
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name))
                ?? "Yazar bilgisi bulunmuyor";

            string materialLabel = BuildMaterialLabel(katalogKaydi);
            string status = DetermineStatus(katalogKaydi);
            string location = DetermineLocation(katalogKaydi);

            return new PublicCatalogRecordDto
            {
                Id = katalogKaydi.Id,
                Title = string.IsNullOrWhiteSpace(katalogKaydi.Baslik) ? "Başlıksız kayıt" : katalogKaydi.Baslik.Trim(),
                Author = author,
                MaterialType = materialLabel,
                Language = string.IsNullOrWhiteSpace(katalogKaydi.Dil) ? null : katalogKaydi.Dil.Trim(),
                PublicationYear = katalogKaydi.YayinYili,
                Isbn = string.IsNullOrWhiteSpace(katalogKaydi.Isbn) ? null : katalogKaydi.Isbn.Trim(),
                Summary = string.IsNullOrWhiteSpace(katalogKaydi.Ozet) ? null : katalogKaydi.Ozet.Trim(),
                Status = status,
                Location = location,
                LastUpdated = katalogKaydi.KayitTarihi
            };
        }

        private static string BuildMaterialLabel(KatalogKaydi katalogKaydi)
        {
            string typeLabel = katalogKaydi.MateryalTuru switch
            {
                MateryalTuru.Kitap => "Kitap",
                MateryalTuru.SureliYayin => "Süreli Yayın",
                MateryalTuru.GorselMateryal => "Görsel Materyal",
                MateryalTuru.Multimedya => "Multimedya",
                MateryalTuru.EKitap => "E-Kitap",
                MateryalTuru.Harita => "Harita",
                MateryalTuru.ElYazmasi => "El Yazması",
                _ => "Materyal"
            };

            if (!string.IsNullOrWhiteSpace(katalogKaydi.MateryalAltTuru))
            {
                return $"{typeLabel} • {katalogKaydi.MateryalAltTuru.Trim()}";
            }

            return typeLabel;
        }

        private static string DetermineStatus(KatalogKaydi katalogKaydi)
        {
            IEnumerable<Nusha> copies = katalogKaydi.Materyaller?
                    .SelectMany(m => m.Nushalar ?? Enumerable.Empty<Nusha>())
                ?? Enumerable.Empty<Nusha>();

            if (copies.Any(n => n.Durumu == NushaDurumu.Rafta))
            {
                return "available";
            }

            if (copies.Any())
            {
                return "limited";
            }

            if (katalogKaydi.Materyaller is { Count: > 0 })
            {
                return "limited";
            }

            return "unavailable";
        }

        private static string DetermineLocation(KatalogKaydi katalogKaydi)
        {
            Materyal? materyal =
                katalogKaydi.Materyaller?
                    .OrderByDescending(m => (m.Nushalar ?? Enumerable.Empty<Nusha>()).Count(n => n.Durumu == NushaDurumu.Rafta))
                    .ThenBy(m => m.Kutuphane != null ? m.Kutuphane.Ad : string.Empty)
                    .FirstOrDefault()
                ?? katalogKaydi.Materyaller?.FirstOrDefault();

            Kutuphane? kutuphane = materyal?.Kutuphane ?? katalogKaydi.Kutuphane;
            if (kutuphane is null)
            {
                return "Katalog deposu";
            }

            string libraryName = !string.IsNullOrWhiteSpace(kutuphane.Ad) ? kutuphane.Ad.Trim() : kutuphane.Kod;

            List<string> regionParts = new();
            if (!string.IsNullOrWhiteSpace(kutuphane.Il))
            {
                regionParts.Add(kutuphane.Il.Trim());
            }
            if (!string.IsNullOrWhiteSpace(kutuphane.Ilce))
            {
                regionParts.Add(kutuphane.Ilce.Trim());
            }

            if (regionParts.Count > 0)
            {
                return $"{libraryName} • {string.Join("/", regionParts)}";
            }

            return libraryName;
        }
    }
}

public class PublicCatalogSearchResponse
{
    public IReadOnlyCollection<PublicCatalogRecordDto> Items { get; set; } = Array.Empty<PublicCatalogRecordDto>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class PublicCatalogRecordDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public string? Language { get; set; }
    public int? PublicationYear { get; set; }
    public string? Isbn { get; set; }
    public string? Summary { get; set; }
    public string Status { get; set; } = "unavailable";
    public string Location { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
