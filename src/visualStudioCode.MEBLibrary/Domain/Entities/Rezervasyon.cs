using Domain.Enums;

namespace Domain.Entities;

public class Rezervasyon
{
    public int Id { get; set; } // Örnekler: 3001, 3055, 4200
    public int KutuphaneId { get; set; } // Örnekler: 205, 310, 512
    public int KullaniciId { get; set; } // Örnekler: 9100, 9125, 9305
    public int MateryalId { get; set; } // Örnekler: 5001, 7820, 9105
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow; // Örnekler: 2024-03-05 09:00, 2024-03-18 15:20, 2024-04-01 08:45
    public DateTime? HazirlanmaTarihi { get; set; } // Örnekler: 2024-03-06 10:30, 2024-03-19 09:15, null
    public DateTime? TamamlanmaTarihi { get; set; } // Örnekler: 2024-03-07 13:00, 2024-03-21 11:45, null
    public RezervasyonDurumu Durumu { get; set; } = RezervasyonDurumu.Beklemede; // Örnekler: RezervasyonDurumu.Beklemede, RezervasyonDurumu.Hazir, RezervasyonDurumu.Tamamlandi
    public string? Not { get; set; } // Örnekler: "Öğrenci sınav sonrası alacak.", "Öğretmen için ayrıldı.", null

    public Kutuphane? Kutuphane { get; set; }
    public Materyal? Materyal { get; set; }
}
