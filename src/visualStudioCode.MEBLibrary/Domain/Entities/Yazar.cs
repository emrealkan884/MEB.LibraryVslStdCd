namespace Domain.Entities;

public class Yazar
{
    public int Id { get; set; } // 1, 64, 205
    public string AdSoyad { get; set; } = string.Empty; // "Orhan Pamuk", "Marie Curie", "Isaac Asimov"
    public string? Ad { get; set; } // "Orhan", "Marie", "Isaac"
    public string? Soyad { get; set; } // "Pamuk", "Curie", "Asimov"
    public DateTime? DogumTarihi { get; set; } // 1952-06-07, 1867-11-07, null
    public string? Uyruk { get; set; } // "Türkiye", "Polonya", "Rusya"
    public string? Aciklama { get; set; } // "Nobel Edebiyat Ödülü 2006", "Çifte Nobel sahibi bilim insanı", "Bilim kurgu yazarı"

    public ICollection<KatalogKaydiYazar> KatalogKaydiYazarlar { get; set; } = new List<KatalogKaydiYazar>();
}
