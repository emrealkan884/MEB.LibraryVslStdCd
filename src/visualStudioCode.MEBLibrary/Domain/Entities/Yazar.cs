using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Yazar : Entity<Guid>
{
    public Yazar()
    {
    }

    public Yazar(Guid id, DateTime? dogumTarihi, string? uyruk, string? aciklama) : base(id)
    {
        DogumTarihi = dogumTarihi;
        Uyruk = uyruk;
        Aciklama = aciklama;
    }

    public string AdSoyad { get; set; } = string.Empty; // "Orhan Pamuk", "Marie Curie", "Isaac Asimov"
    public DateTime? DogumTarihi { get; set; } // 1952-06-07, 1867-11-07, null
    public string? Uyruk { get; set; } // "Türkiye", "Polonya", "Rusya"
    public string? Aciklama { get; set; } // "Nobel Edebiyat Ödülü 2006", "Çifte Nobel sahibi bilim insanı", "Bilim kurgu yazarı"

    
    //Navigation properties
    public ICollection<KatalogKaydiYazar> KatalogKaydiYazarlar { get; set; } = new List<KatalogKaydiYazar>();
}