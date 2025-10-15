using Domain.Enums;

namespace Domain.Entities;

public class Rezervasyon
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public int KullaniciId { get; set; }
    public int MateryalId { get; set; }
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;
    public DateTime? HazirlanmaTarihi { get; set; }
    public DateTime? TamamlanmaTarihi { get; set; }
    public RezervasyonDurumu Durumu { get; set; } = RezervasyonDurumu.Beklemede;
    public string? Not { get; set; }

    public Kutuphane? Kutuphane { get; set; }
    public Materyal? Materyal { get; set; }
}
