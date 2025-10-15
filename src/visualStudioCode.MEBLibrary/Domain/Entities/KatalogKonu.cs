namespace Domain.Entities;

public class KatalogKonu
{
    public int Id { get; set; } // Örnekler: 1, 8, 24
    public int KatalogKaydiId { get; set; } // Örnekler: 120, 350, 875
    public string KonuBasligi { get; set; } = string.Empty; // Örnekler: "STEM egitimi", "Osmanli sanati", "Robotik"
    public int? OtoriteKaydiId { get; set; } // Örnekler: 600, 725, null

    public KatalogKaydi? KatalogKaydi { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}
