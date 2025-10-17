using Domain.Entities;
using Domain.Enums;

namespace Application.Features.OduncIslemler.Commands.Extend;

public class ExtendedOduncIslemiResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? UzatmaTarihi { get; set; }
    public int UzatmaSayisi { get; set; }
    public OduncDurumu Durumu { get; set; }
    public string? Not { get; set; }
}