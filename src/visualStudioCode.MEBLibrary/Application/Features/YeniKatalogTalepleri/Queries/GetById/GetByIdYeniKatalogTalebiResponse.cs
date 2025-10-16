using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.YeniKatalogTalepleri.Queries.GetById;

public class GetByIdYeniKatalogTalebiResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid TalepEdenKutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? AltBaslik { get; set; }
    public string? YazarMetni { get; set; }
    public string? Isbn { get; set; }
    public string? Issn { get; set; }
    public string? MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public string? Dil { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public string? Aciklama { get; set; }
    public string? RedGerekcesi { get; set; }
    public TalepDurumu Durum { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
    public Guid? KatalogKaydiId { get; set; }
    public MateryalTuru? SuggestedMateryalTuru { get; set; }
    public string? SuggestedMateryalAltTuru { get; set; }
}
