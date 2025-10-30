using Application.Features.KatalogKayitlari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.PublicCatalog.Queries.GetDetail;

public class GetDetailPublicCatalogQuery : IRequest<PublicCatalogDetailResponse>
{
    public Guid Id { get; set; }

    public class GetDetailPublicCatalogQueryHandler : IRequestHandler<GetDetailPublicCatalogQuery, PublicCatalogDetailResponse>
    {
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;

        public GetDetailPublicCatalogQueryHandler(IKatalogKaydiRepository katalogKaydiRepository)
        {
            _katalogKaydiRepository = katalogKaydiRepository;
        }

        public async Task<PublicCatalogDetailResponse> Handle(GetDetailPublicCatalogQuery request, CancellationToken cancellationToken)
        {
            KatalogKaydi? katalog = await _katalogKaydiRepository.GetAsync(
                predicate: k => k.Id == request.Id,
                include: query => query
                    .Include(k => k.KatalogYazarlar)
                        .ThenInclude(ky => ky.Yazar)
                    .Include(k => k.DeweySiniflama)
                    .Include(k => k.FormatDetaylari)
                    .Include(k => k.Materyaller)
                        .ThenInclude(m => m.Kutuphane)
                    .Include(k => k.Materyaller)
                        .ThenInclude(m => m.Bolum)
                    .Include(k => k.Materyaller)
                        .ThenInclude(m => m.Nushalar),
                cancellationToken: cancellationToken);

            if (katalog is null)
                throw new BusinessException(KatalogKaydisBusinessMessages.KatalogKaydiNotExists);

            List<string> authors = (katalog.KatalogYazarlar ?? new List<KatalogKaydiYazar>())
                .OrderBy(y => y.Sira)
                .Select(y => y.Yazar?.AdSoyad?.Trim())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .ToList();

            List<PublicCatalogDigitalResourceDto> digitalResources = (katalog.FormatDetaylari ?? new List<MateryalFormatDetay>())
                .Where(detail => !string.IsNullOrWhiteSpace(detail.ErişimBilgisi))
                .Select(detail => new PublicCatalogDigitalResourceDto
                {
                    Format = detail.FormatBilgisi ?? detail.MateryalTuru.ToString(),
                    AccessInformation = detail.ErişimBilgisi!.Trim(),
                    Language = detail.Dil?.Trim(),
                    Description = detail.FizikselTanimi?.Trim()
                })
                .ToList();

            List<PublicCatalogAvailabilityDto> availability = BuildAvailability(katalog);

            PublicCatalogDetailResponse response = new()
            {
                Id = katalog.Id,
                Title = katalog.Baslik.Trim(),
                Subtitle = katalog.AltBaslik?.Trim(),
                Authors = authors,
                MaterialType = BuildMaterialLabel(katalog),
                MaterialCategory = katalog.MateryalTuru.ToString(),
                Language = katalog.Dil?.Trim(),
                PublicationYear = katalog.YayinYili,
                Publisher = katalog.Yayinevi?.Trim(),
                PublicationPlace = katalog.YayinYeri?.Trim(),
                Isbn = katalog.Isbn?.Trim(),
                Summary = katalog.Ozet?.Trim(),
                Notes = katalog.Notlar?.Trim(),
                DeweyCode = katalog.DeweySiniflama?.Kod?.Trim(),
                DeweyTitle = katalog.DeweySiniflama?.Baslik?.Trim(),
                Marc21 = katalog.Marc21Verisi,
                UpdatedAt = katalog.KayitTarihi,
                DigitalResources = digitalResources,
                Availability = availability
            };

            return response;
        }

        private static List<PublicCatalogAvailabilityDto> BuildAvailability(KatalogKaydi katalog)
        {
            List<Materyal> materyaller = new List<Materyal>(katalog.Materyaller) ?? new List<Materyal>();

            IEnumerable<PublicCatalogAvailabilityDto> libraryEntries = materyaller
                .Where(materyal => materyal.Kutuphane is not null)
                .Select(materyal => new
                {
                    Library = materyal.Kutuphane!,
                    Material = materyal,
                    Copies = materyal.Nushalar ?? new List<Nusha>()
                })
                .GroupBy(x => x.Library.Id)
                .Select(group =>
                {
                    Kutuphane library = group.First().Library;
                    IEnumerable<Nusha> allCopies = group.SelectMany(x => x.Copies);

                    int totalCopies = allCopies.Count();
                    int availableCopies = allCopies.Count(copy => copy.Durumu == NushaDurumu.Rafta);

                    HashSet<string> sections = group
                        .Select(x => x.Material.Bolum?.Ad?.Trim())
                        .Where(section => !string.IsNullOrWhiteSpace(section))
                        .Select(section => section!)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    bool isReservable = group.Any(x => x.Material.RezervasyonaAcik);

                    return new PublicCatalogAvailabilityDto
                    {
                        LibraryId = library.Id,
                        LibraryName = string.IsNullOrWhiteSpace(library.Ad) ? library.Kod : library.Ad.Trim(),
                        City = library.Il?.Trim(),
                        District = library.Ilce?.Trim(),
                        Address = library.Adres?.Trim(),
                        Sections = sections.ToList(),
                        IsReservable = isReservable,
                        TotalCopies = totalCopies,
                        AvailableCopies = availableCopies,
                        Notes = group.Select(x => x.Material.Not).FirstOrDefault(note => !string.IsNullOrWhiteSpace(note))?.Trim()
                    };
                });

            return libraryEntries
                .OrderByDescending(entry => entry.AvailableCopies)
                .ThenBy(entry => entry.LibraryName)
                .ToList();
        }

        private static string BuildMaterialLabel(KatalogKaydi katalog)
        {
            string typeLabel = katalog.MateryalTuru switch
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

            if (!string.IsNullOrWhiteSpace(katalog.MateryalAltTuru))
            {
                return $"{typeLabel} • {katalog.MateryalAltTuru.Trim()}";
            }

            return typeLabel;
        }
    }
}







