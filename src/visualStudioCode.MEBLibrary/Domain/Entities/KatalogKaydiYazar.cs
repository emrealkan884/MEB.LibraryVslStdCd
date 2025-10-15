using Domain.Enums;

namespace Domain.Entities;

public class KatalogKaydiYazar
{
    public int Id { get; set; } // Örnekler: 1, 4, 12
    public int KatalogKaydiId { get; set; } // Örnekler: 120, 350, 875
    public int YazarId { get; set; } // Örnekler: 45, 78, 210
    public YazarRolu Rol { get; set; } = YazarRolu.Yazar; // Örnekler: YazarRolu.Yazar, YazarRolu.Cevirmen, YazarRolu.Editor
    public int Sira { get; set; } = 1; // Örnekler: 1, 2, 3
    public int? OtoriteKaydiId { get; set; } // Örnekler: 500, 820, null

    public KatalogKaydi? KatalogKaydi { get; set; }
    public Yazar? Yazar { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}
