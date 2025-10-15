using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKayitlari.Queries.GetById;

public class GetByIdKatalogKaydiResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid? DeweySiniflamaId { get; set; }
    public Guid? YeniKatalogTalebiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? AltBaslik { get; set; }
    public string? Isbn { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public int? SayfaSayisi { get; set; }
    public string? Dil { get; set; }
    public string? Dizi { get; set; }
    public string? Baski { get; set; }
    public string? Ozet { get; set; }
    public string? Notlar { get; set; }
    public string? KapakResmiYolu { get; set; }
    public MateryalTuru MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public string? Marc21Verisi { get; set; }
    public bool RdaUyumlu { get; set; }
    public DateTime KayitTarihi { get; set; }
}
