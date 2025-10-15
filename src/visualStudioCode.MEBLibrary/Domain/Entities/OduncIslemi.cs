using Domain.Enums;

namespace Domain.Entities;

public class OduncIslemi
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public int KullaniciId { get; set; }
    public int NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; } = DateTime.UtcNow;
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public OduncDurumu Durumu { get; set; } = OduncDurumu.Aktif;
    public string? Not { get; set; }

    public Kutuphane? Kutuphane { get; set; }
    public Nusha? Nusha { get; set; }
}
