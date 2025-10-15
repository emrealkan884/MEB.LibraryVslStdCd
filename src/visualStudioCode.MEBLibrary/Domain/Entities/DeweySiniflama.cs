namespace Domain.Entities;

public class DeweySiniflama
{
    public int Id { get; set; } // 500, 813, 920
    public string Kod { get; set; } = string.Empty; // "500", "510", "516.2"
    public string Baslik { get; set; } = string.Empty; // "Fen Bilimleri", "Matematik", "Geometri"
    public string? Aciklama { get; set; } // "Doğa bilimleri genel", "Cebir ve sayılar", null
    public int? UstSinifId { get; set; } // null, 500, 510

    public DeweySiniflama? UstSinif { get; set; }
    public ICollection<DeweySiniflama> AltSiniflar { get; set; } = new List<DeweySiniflama>();
    public ICollection<KatalogKaydi> KatalogKayitlari { get; set; } = new List<KatalogKaydi>();
}
