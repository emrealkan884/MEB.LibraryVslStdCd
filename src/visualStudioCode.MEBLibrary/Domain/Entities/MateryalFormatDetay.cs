using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class MateryalFormatDetay : Entity<Guid>
{
    public Guid KatalogKaydiId { get; set; } // 1001, 2045, 3500
    public MateryalTuru MateryalTuru { get; set; } = MateryalTuru.Kitap; // MateryalTuru.Kitap, MateryalTuru.GorselMateryal, MateryalTuru.EKitap
    public string? FizikselTanimi { get; set; } // "320 s., resimli ; 24 cm", "2 cilt, 30 cm", "1 poster ; 60 x 90 cm"
    public string? SureBilgisi { get; set; } // "90 dk.", "2 DVD, toplam 180 dk.", "45 dk. MP3"
    public string? FormatBilgisi { get; set; } // "PDF", "MP4", "Ciltli"
    public string? Dil { get; set; } // "Türkçe", "İngilizce", "Fransızca"
    public string? ErişimBilgisi { get; set; } // "https://kutuphane.meb.gov/stem.pdf", "Erişim: Okul intraneti", "Sınıf kitaplığında DVD"

    //Navigation properties
    public KatalogKaydi? KatalogKaydi { get; set; }
}
