namespace Domain.Entities;

public class DeweySiniflama
{
    public int Id { get; set; } // Örnekler: 500, 813, 920
    public string Kod { get; set; } = string.Empty; // Örnekler: "500", "510", "516.2"
    public string Baslik { get; set; } = string.Empty; // Örnekler: "Fen Bilimleri", "Matematik", "Geometri"
    public string? Aciklama { get; set; } // Örnekler: "Doğa bilimleri genel", "Cebir ve sayılar", null
    public int? UstSinifId { get; set; } // Örnekler: null, 500, 510

    public DeweySiniflama? UstSinif { get; set; }
    public ICollection<DeweySiniflama> AltSiniflar { get; set; } = new List<DeweySiniflama>();
    public ICollection<KatalogKaydi> KatalogKayitlari { get; set; } = new List<KatalogKaydi>();
}
