using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Materyal : Entity<Guid>
{
    public Materyal()
    {
    }

    public Materyal(Guid id, Guid katalogKaydiId, Guid kutuphaneId, Guid? kutuphaneBolumuId, MateryalFormati formati,
        bool rezervasyonaAcik, int maksimumOduncSuresiGun, string? not, KatalogKaydi? katalogKaydi,
        Kutuphane? kutuphane, KutuphaneBolumu? bolum) : base(id)
    {
        KatalogKaydiId = katalogKaydiId;
        KutuphaneId = kutuphaneId;
        KutuphaneBolumuId = kutuphaneBolumuId;
        Formati = formati;
        RezervasyonaAcik = rezervasyonaAcik;
        MaksimumOduncSuresiGun = maksimumOduncSuresiGun;
        Not = not;
        KatalogKaydi = katalogKaydi;
        Kutuphane = kutuphane;
        Bolum = bolum;
    }

    public Guid KatalogKaydiId { get; set; } // 120, 350, 875
    public Guid KutuphaneId { get; set; } // 10, 205, 512
    public Guid? KutuphaneBolumuId { get; set; } // 45, 78, null

    public MateryalFormati
        Formati { get; set; } // MateryalFormati.Kitap, MateryalFormati.SureliYayin, MateryalFormati.Multimedya

    public bool RezervasyonaAcik { get; set; } = true; // true, false, true
    public int MaksimumOduncSuresiGun { get; set; } = 15; // 15, 7, 30
    public string? Not { get; set; } // "Sadece öğretmenler ödünç alabilir.", "Yeni baskı bekleniyor.", null

    //Navigation properties
    public KatalogKaydi? KatalogKaydi { get; set; }
    public Kutuphane? Kutuphane { get; set; }
    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
    public ICollection<MateryalEtiket> Etiketler { get; set; } = new List<MateryalEtiket>();
}