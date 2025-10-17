using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class DeweySiniflama : Entity<Guid>
{
    public DeweySiniflama()
    {
    }

    public DeweySiniflama(Guid id, string? aciklama, Guid? ustSinifId, DeweySiniflama? ustSinif) : base(id)
    {
        Aciklama = aciklama;
        UstSinifId = ustSinifId;
        UstSinif = ustSinif;
    }

    public string Kod { get; set; } = string.Empty; // "500", "510", "516.2"
    public string Baslik { get; set; } = string.Empty; // "Fen Bilimleri", "Matematik", "Geometri"
    public string? Aciklama { get; set; } // "Doğa bilimleri genel", "Cebir ve sayılar", null
    public Guid? UstSinifId { get; set; } // null, 500, 510

    //Navigation properties
    public DeweySiniflama? UstSinif { get; set; }
    public ICollection<DeweySiniflama> AltSiniflar { get; set; } = new List<DeweySiniflama>();
    public ICollection<KatalogKaydi> KatalogKayitlari { get; set; } = new List<KatalogKaydi>();
}