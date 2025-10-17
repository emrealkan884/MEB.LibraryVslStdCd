using Domain.Entities;
using Domain.Enums;

namespace Application.Features.OduncIslemler.Commands.Return;

public class ReturnedOduncIslemiResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime IadeTarihi { get; set; }
    public OduncDurumu Durumu { get; set; }
    public decimal? GecikmeCezaMiktari { get; set; }
    public int? GecikmeGunSayisi { get; set; }
    public string? Not { get; set; }
}