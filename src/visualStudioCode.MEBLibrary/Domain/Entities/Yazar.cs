namespace Domain.Entities;

public class Yazar
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? Uyruk { get; set; }
    public string? Aciklama { get; set; }

    public ICollection<KatalogKaydiYazar> KatalogKaydiYazarlar { get; set; } = new List<KatalogKaydiYazar>();
}
