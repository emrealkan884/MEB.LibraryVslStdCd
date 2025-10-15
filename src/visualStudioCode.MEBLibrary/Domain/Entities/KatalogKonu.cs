namespace Domain.Entities;

public class KatalogKonu
{
    public int Id { get; set; } // 1, 8, 24
    public int KatalogKaydiId { get; set; } // 120, 350, 875
    public string KonuBasligi { get; set; } = string.Empty; // "STEM egitimi", "Osmanli sanati", "Robotik"
    public int? OtoriteKaydiId { get; set; } // 600, 725, null

    public KatalogKaydi? KatalogKaydi { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}
