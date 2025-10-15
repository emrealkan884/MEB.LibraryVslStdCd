namespace Domain.Entities;

public class Yazar
{
    public int Id { get; set; } // Örnekler: 1, 64, 205
    public string AdSoyad { get; set; } = string.Empty; // Örnekler: "Orhan Pamuk", "Marie Curie", "Isaac Asimov"
    public string? Ad { get; set; } // Örnekler: "Orhan", "Marie", "Isaac"
    public string? Soyad { get; set; } // Örnekler: "Pamuk", "Curie", "Asimov"
    public DateTime? DogumTarihi { get; set; } // Örnekler: 1952-06-07, 1867-11-07, null
    public string? Uyruk { get; set; } // Örnekler: "Türkiye", "Polonya", "Rusya"
    public string? Aciklama { get; set; } // Örnekler: "Nobel Edebiyat Ödülü 2006", "Çifte Nobel sahibi bilim insanı", "Bilim kurgu yazarı"

    public ICollection<KatalogKaydiYazar> KatalogKaydiYazarlar { get; set; } = new List<KatalogKaydiYazar>();
}
