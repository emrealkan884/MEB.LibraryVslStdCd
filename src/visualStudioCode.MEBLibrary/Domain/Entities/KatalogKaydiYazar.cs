using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class KatalogKaydiYazar:Entity<Guid>
{
    public int KatalogKaydiId { get; set; } // 120, 350, 875
    public int YazarId { get; set; } // 45, 78, 210
    public YazarRolu Rol { get; set; } = YazarRolu.Yazar; // YazarRolu.Yazar, YazarRolu.Cevirmen, YazarRolu.Editor
    public int Sira { get; set; } = 1; // 1, 2, 3
    public int? OtoriteKaydiId { get; set; } // 500, 820, null

    public KatalogKaydi? KatalogKaydi { get; set; }
    public Yazar? Yazar { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}
