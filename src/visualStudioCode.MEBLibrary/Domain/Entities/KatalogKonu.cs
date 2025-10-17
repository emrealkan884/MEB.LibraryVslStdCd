using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class KatalogKonu : Entity<Guid>
{
    public KatalogKonu()
    {
    }

    public KatalogKonu(Guid id, Guid katalogKaydiId, Guid? otoriteKaydiId, KatalogKaydi? katalogKaydi,
        OtoriteKaydi? otoriteKaydi) : base(id)
    {
        KatalogKaydiId = katalogKaydiId;
        OtoriteKaydiId = otoriteKaydiId;
        KatalogKaydi = katalogKaydi;
        OtoriteKaydi = otoriteKaydi;
    }

    public Guid KatalogKaydiId { get; set; } // 120, 350, 875
    public string KonuBasligi { get; set; } = string.Empty; // "STEM egitimi", "Osmanli sanati", "Robotik"
    public Guid? OtoriteKaydiId { get; set; } // 600, 725, null

    //Navigation properties
    public KatalogKaydi? KatalogKaydi { get; set; }
    public OtoriteKaydi? OtoriteKaydi { get; set; }
}