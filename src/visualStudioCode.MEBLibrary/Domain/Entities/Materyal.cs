using Domain.Enums;

namespace Domain.Entities;

public class Materyal
{
    public int Id { get; set; }
    public int KatalogKaydiId { get; set; }
    public int KutuphaneId { get; set; }
    public int? KutuphaneBolumuId { get; set; }
    public MateryalFormati Formati { get; set; }
    public bool RezervasyonaAcik { get; set; } = true;
    public int MaksimumOduncSuresiGun { get; set; } = 15;
    public string? Not { get; set; }

    public KatalogKaydi? KatalogKaydi { get; set; }
    public Kutuphane? Kutuphane { get; set; }
    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
    public ICollection<MateryalEtiket> Etiketler { get; set; } = new List<MateryalEtiket>();
}
