using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class KatalogKaydiYazar : Entity<Guid>
{
    public KatalogKaydiYazar()
    {
    }

    public KatalogKaydiYazar(Guid id, Guid katalogKaydiId, Guid yazarId, YazarRolu rol, int sira, Guid? otoriteKaydiId,
        KatalogKaydi? katalogKaydi, Yazar? yazar, OtoriteKaydi? otoriteKaydi) : base(id)
    {
        KatalogKaydiId = katalogKaydiId;
        YazarId = yazarId;
        Rol = rol;
        Sira = sira;
        OtoriteKaydiId = otoriteKaydiId;
        KatalogKaydi = katalogKaydi;
        Yazar = yazar;
        OtoriteKaydi = otoriteKaydi;
    }

    public Guid KatalogKaydiId { get; set; } // 120, 350, 875
    public Guid YazarId { get; set; } // 45, 78, 210
    public YazarRolu Rol { get; set; } = YazarRolu.Yazar; // YazarRolu.Yazar, YazarRolu.Cevirmen, YazarRolu.Editor
    public int Sira { get; set; } = 1; // 1, 2, 3
    public Guid? OtoriteKaydiId { get; set; } // 500, 820, null

    //Navigation properties
    public KatalogKaydi? KatalogKaydi { get; set; }
    public Yazar? Yazar { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}