namespace Domain.Entities;

public class Etkinlik
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public string? Konum { get; set; }
    public string? AfisDosyasi { get; set; }

    public Kutuphane? Kutuphane { get; set; }
}
