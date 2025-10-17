using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Rezervasyon : Entity<Guid>
{
    public Rezervasyon()
    {
    }

    public Rezervasyon(Guid id, Guid kutuphaneId, Guid kullaniciId, Guid materyalId, DateTime? hazirlanmaTarihi,
        DateTime? tamamlanmaTarihi, RezervasyonDurumu durumu, string? not, Kutuphane? kutuphane,
        Materyal? materyal) : base(id)
    {
        KutuphaneId = kutuphaneId;
        KullaniciId = kullaniciId;
        MateryalId = materyalId;
        HazirlanmaTarihi = hazirlanmaTarihi;
        TamamlanmaTarihi = tamamlanmaTarihi;
        Durumu = durumu;
        Not = not;
        Kutuphane = kutuphane;
        Materyal = materyal;
    }

    public Guid KutuphaneId { get; set; } // 205, 310, 512
    public Guid KullaniciId { get; set; } // 9100, 9125, 9305
    public Guid MateryalId { get; set; } // 5001, 7820, 9105
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow; // 2024-03-05 09:00, 2024-03-18 15:20, 2024-04-01 08:45
    public DateTime? HazirlanmaTarihi { get; set; } // 2024-03-06 10:30, 2024-03-19 09:15, null
    public DateTime? TamamlanmaTarihi { get; set; } // 2024-03-07 13:00, 2024-03-21 11:45, null

    public RezervasyonDurumu Durumu { get; set; } =
        RezervasyonDurumu
            .Beklemede; // RezervasyonDurumu.Beklemede, RezervasyonDurumu.Hazir, RezervasyonDurumu.Tamamlandi

    public string? Not { get; set; } // "Öğrenci sınav sonrası alacak.", "Öğretmen için ayrıldı.", null

    //Navigation properties
    public Kutuphane? Kutuphane { get; set; }
    public Materyal? Materyal { get; set; }
}