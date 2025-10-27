using Application.Features.KatalogKayitlari.Commands.Create;
using Application.Features.KatalogKayitlari.Commands.Delete;
using Application.Features.KatalogKayitlari.Commands.Update;
using Application.Features.KatalogKayitlari.Queries.GetById;
using Application.Features.KatalogKayitlari.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.KatalogKayitlari.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateKatalogKaydiCommand, KatalogKaydi>()
            .ForMember(dest => dest.KatalogYazarlar, opt => opt.Ignore())
            .ForMember(dest => dest.Konular, opt => opt.Ignore())
            .ForMember(dest => dest.MarcAlanlari, opt => opt.MapFrom(src => src.MarcAlanlari));;

        CreateMap<KatalogKaydi, CreatedKatalogKaydiResponse>()
            .ForMember(dest => dest.Yazarlar, opt => opt.Ignore())
            .ForMember(dest => dest.Konular, opt => opt.Ignore());

        CreateMap<UpdateKatalogKaydiCommand, KatalogKaydi>();
        CreateMap<KatalogKaydi, UpdatedKatalogKaydiResponse>();

        CreateMap<DeleteKatalogKaydiCommand, KatalogKaydi>();
        CreateMap<KatalogKaydi, DeletedKatalogKaydiResponse>();

        CreateMap<KatalogKaydi, GetByIdKatalogKaydiResponse>();

        CreateMap<KatalogKaydi, GetListKatalogKaydiListItemDto>()
            .AfterMap((src, dest) =>
            {
                var yazarlar = src.KatalogYazarlar ?? new List<KatalogKaydiYazar>();
                dest.Yazarlar = yazarlar
                    .OrderBy(y => y.Sira)
                    .Select(y => y.Yazar?.AdSoyad?.Trim())
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Select(name => name!)
                    .ToList();

                dest.DeweyKod = src.DeweySiniflama?.Kod;
                dest.DeweyBaslik = src.DeweySiniflama?.Baslik;

                var materyaller = src.Materyaller ?? new List<Materyal>();
                var allCopies = materyaller.SelectMany(m => m.Nushalar ?? new List<Nusha>());
                dest.NushaSayisiToplam = allCopies.Count();
                dest.NushaSayisiMusait = allCopies.Count(n => n.Durumu == NushaDurumu.Rafta);

                dest.Lokasyonlar = materyaller
                    .SelectMany(m =>
                    {
                        var kutuphane = m.Kutuphane;
                        if (kutuphane is null)
                            return Enumerable.Empty<LocationProjection>();

                        var nushalar = m.Nushalar ?? new List<Nusha>();

                        if (nushalar.Any())
                        {
                            return nushalar.Select(nusha => new LocationProjection(
                                kutuphane,
                                m.Bolum?.Ad?.Trim(),
                                nusha.Raf?.Kod?.Trim(),
                                Toplam: 1,
                                Musait: nusha.Durumu == NushaDurumu.Rafta ? 1 : 0));
                        }

                        // Materyalin henüz nüshası yoksa toplam/müsait bilgisini 0 olarak taşı.
                        return new[]
                        {
                            new LocationProjection(
                                kutuphane,
                                m.Bolum?.Ad?.Trim(),
                                null,
                                Toplam: 0,
                                Musait: 0)
                        };
                    })
                    .Where(p => p.Kutuphane is not null)
                    .GroupBy(p => new
                    {
                        LibraryId = p.Kutuphane!.Id,
                        p.Bolum,
                        p.Raf
                    })
                    .Select(group =>
                    {
                        LocationProjection sample = group.First();
                        int toplam = group.Sum(x => x.Toplam);
                        int musait = group.Sum(x => x.Musait);

                        if (musait == 0)
                            return null;

                        return BuildDetailedLocationLabel(
                            sample.Kutuphane!,
                            group.Key.Bolum,
                            group.Key.Raf,
                            musait,
                            toplam);
                    })
                    .Where(label => !string.IsNullOrWhiteSpace(label))
                    .Select(label => label!)
                    .ToList();
            });
        CreateMap<IPaginate<KatalogKaydi>, GetListResponse<GetListKatalogKaydiListItemDto>>();
    }

    private readonly record struct LocationProjection(
        Kutuphane? Kutuphane,
        string? Bolum,
        string? Raf,
        int Toplam,
        int Musait);

    private static string? BuildDetailedLocationLabel(
        Kutuphane kutuphane,
        string? bolum,
        string? raf,
        int musait,
        int toplam)
    {
        string libraryName = string.IsNullOrWhiteSpace(kutuphane.Ad)
            ? kutuphane.Kod
            : kutuphane.Ad.Trim();

        string locationSegment = BuildLocationSegment(kutuphane.Il, kutuphane.Ilce);

        List<string> parts = new();

        if (!string.IsNullOrWhiteSpace(locationSegment))
            parts.Add($"{libraryName} ({locationSegment})");
        else
            parts.Add(libraryName);

        string shelfSegment = BuildShelfSegment(bolum, raf);
        if (!string.IsNullOrWhiteSpace(shelfSegment))
            parts.Add(shelfSegment);

        parts.Add($"Müsait: {musait}/{toplam}");

        return string.Join(" - ", parts);
    }

    private static string BuildLocationSegment(string? il, string? ilce)
    {
        List<string> pieces = new();
        if (!string.IsNullOrWhiteSpace(il))
            pieces.Add(il.Trim());
        if (!string.IsNullOrWhiteSpace(ilce))
            pieces.Add(ilce.Trim());
        return string.Join("/", pieces);
    }

    private static string BuildShelfSegment(string? bolum, string? raf)
    {
        List<string> pieces = new();
        if (!string.IsNullOrWhiteSpace(bolum))
            pieces.Add(bolum);
        if (!string.IsNullOrWhiteSpace(raf))
            pieces.Add(raf);
        return string.Join(" / ", pieces);
    }
}
