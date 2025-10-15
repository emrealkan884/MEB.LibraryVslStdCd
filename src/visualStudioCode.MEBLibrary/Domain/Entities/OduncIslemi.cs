using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class OduncIslemi : Entity<Guid>
{
    public Guid KutuphaneId { get; set; } // 205, 310, 512
    public Guid KullaniciId { get; set; } // 9001, 9050, 9205
    public Guid NushaId { get; set; } // 1, 75, 420
    public DateTime AlinmaTarihi { get; set; } = DateTime.UtcNow; // 2024-03-01 09:20, 2024-03-15 14:10, 2024-04-02 11:45
    public DateTime SonTeslimTarihi { get; set; } // 2024-03-15, 2024-03-22, 2024-04-16
    public DateTime? IadeTarihi { get; set; } // 2024-03-14, 2024-03-25, null
    public OduncDurumu Durumu { get; set; } = OduncDurumu.Aktif; // OduncDurumu.Aktif, OduncDurumu.IadeEdildi, OduncDurumu.Gecikmis
    public string? Not { get; set; } // "Öğretmen talebi", "Gecikme hatırlatması gönderildi", null

    //Navigation properties
    public Kutuphane? Kutuphane { get; set; }
    public Nusha? Nusha { get; set; }
}
