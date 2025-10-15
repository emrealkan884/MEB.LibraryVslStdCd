using Domain.Enums;

namespace Domain.Entities;

public class Materyal
{
    public int Id { get; set; } // Örnekler: 5001, 7820, 9105
    public int KatalogKaydiId { get; set; } // Örnekler: 120, 350, 875
    public int KutuphaneId { get; set; } // Örnekler: 10, 205, 512
    public int? KutuphaneBolumuId { get; set; } // Örnekler: 45, 78, null
    public MateryalFormati Formati { get; set; } // Örnekler: MateryalFormati.Kitap, MateryalFormati.SureliYayin, MateryalFormati.Multimedya
    public bool RezervasyonaAcik { get; set; } = true; // Örnekler: true, false, true
    public int MaksimumOduncSuresiGun { get; set; } = 15; // Örnekler: 15, 7, 30
    public string? Not { get; set; } // Örnekler: "Sadece öğretmenler ödünç alabilir.", "Yeni baskı bekleniyor.", null

    public KatalogKaydi? KatalogKaydi { get; set; }
    public Kutuphane? Kutuphane { get; set; }
    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
    public ICollection<MateryalEtiket> Etiketler { get; set; } = new List<MateryalEtiket>();
}
