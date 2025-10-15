namespace Domain.Entities;

public class Etkinlik
{
    public int Id { get; set; } // Örnekler: 15, 32, 78
    public int KutuphaneId { get; set; } // Örnekler: 205, 310, 512
    public string Baslik { get; set; } = string.Empty; // Örnekler: "Kitap Okuma Kulübü", "STEM Atölyesi", "Yazar Buluşması"
    public string? Aciklama { get; set; } // Örnekler: "9. sınıf öğrencileri için okuma tartışması.", "Robotik kodlama atölyesi.", null
    public DateTime BaslangicTarihi { get; set; } // Örnekler: 2024-03-12 10:00, 2024-04-05 14:30, 2024-05-20 09:00
    public DateTime BitisTarihi { get; set; } // Örnekler: 2024-03-12 12:00, 2024-04-05 17:00, 2024-05-20 11:30
    public string? Konum { get; set; } // Örnekler: "Konferans Salonu", "Fen Laboratuvarı", "Çevrim içi"
    public string? AfisDosyasi { get; set; } // Örnekler: "/etkinlikler/stem-atolye.jpg", "https://cdn.okul.gov/afis.pdf", null

    public Kutuphane? Kutuphane { get; set; }
}
