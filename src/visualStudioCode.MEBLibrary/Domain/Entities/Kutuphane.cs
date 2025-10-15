using Domain.Enums;

namespace Domain.Entities;

public class Kutuphane
{
    public int Id { get; set; }
    public string Kod { get; set; } = string.Empty;
    public string Ad { get; set; } = string.Empty;
    public KutuphaneTipi Tip { get; set; }
    public string Adres { get; set; } = string.Empty;
    public string? Telefon { get; set; }
    public string? EPosta { get; set; }
    public bool Aktif { get; set; } = true;

    public ICollection<KatalogKaydi> KatalogKayitlari { get; set; } = new List<KatalogKaydi>();
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
    public ICollection<Etkinlik> Etkinlikler { get; set; } = new List<Etkinlik>();
    public ICollection<KutuphaneBolumu> Bolumler { get; set; } = new List<KutuphaneBolumu>();
}
