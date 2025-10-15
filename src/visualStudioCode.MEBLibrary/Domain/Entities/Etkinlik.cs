using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Etkinlik: Entity<Guid>
{
    public Guid KutuphaneId { get; set; } // 205, 310, 512
    public string Baslik { get; set; } = string.Empty; // "Kitap Okuma Kulübü", "STEM Atölyesi", "Yazar Buluşması"
    public string? Aciklama { get; set; } // "9. sınıf öğrencileri için okuma tartışması.", "Robotik kodlama atölyesi.", null
    public DateTime BaslangicTarihi { get; set; } // 2024-03-12 10:00, 2024-04-05 14:30, 2024-05-20 09:00
    public DateTime BitisTarihi { get; set; } // 2024-03-12 12:00, 2024-04-05 17:00, 2024-05-20 11:30
    public string? Konum { get; set; } // "Konferans Salonu", "Fen Laboratuvarı", "Çevrim içi"
    public string? AfisDosyasi { get; set; } // "/etkinlikler/stem-atolye.jpg", "https://cdn.okul.gov/afis.pdf", null

    //Navigation properties
    public Kutuphane? Kutuphane { get; set; }
}
