using Domain.Enums;

namespace Domain.Entities;

public class OduncIslemi
{
    public int Id { get; set; } // Örnekler: 10001, 12045, 15032
    public int KutuphaneId { get; set; } // Örnekler: 205, 310, 512
    public int KullaniciId { get; set; } // Örnekler: 9001, 9050, 9205
    public int NushaId { get; set; } // Örnekler: 1, 75, 420
    public DateTime AlinmaTarihi { get; set; } = DateTime.UtcNow; // Örnekler: 2024-03-01 09:20, 2024-03-15 14:10, 2024-04-02 11:45
    public DateTime SonTeslimTarihi { get; set; } // Örnekler: 2024-03-15, 2024-03-22, 2024-04-16
    public DateTime? IadeTarihi { get; set; } // Örnekler: 2024-03-14, 2024-03-25, null
    public OduncDurumu Durumu { get; set; } = OduncDurumu.Aktif; // Örnekler: OduncDurumu.Aktif, OduncDurumu.IadeEdildi, OduncDurumu.Gecikmis
    public string? Not { get; set; } // Örnekler: "Öğretmen talebi", "Gecikme hatırlatması gönderildi", null

    public Kutuphane? Kutuphane { get; set; }
    public Nusha? Nusha { get; set; }
}
