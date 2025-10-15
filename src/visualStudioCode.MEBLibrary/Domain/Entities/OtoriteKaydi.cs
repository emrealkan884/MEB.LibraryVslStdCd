using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class OtoriteKaydi : Entity<Guid>
{
    public string YetkiliBaslik { get; set; } = string.Empty; // "STEM egitimi", "Pamuk, Orhan", "Turkiye Cumhuriyeti Milli Egitim Bakanligi"
    public OtoriteTuru OtoriteTuru { get; set; } = OtoriteTuru.KonuBasligi; // OtoriteTuru.KonuBasligi, OtoriteTuru.Kisi, OtoriteTuru.Kurum
    public string? AlternatifBasliklar { get; set; } // "Fen bilimleri egitimi; STEM education", "MEB; Milli Egitim Bakanligi", null
    public string? BagliTerimler { get; set; } // "Fen bilimleri; Proje tabanli ogrenme", "Osmanli sanati; Minyatur", null
    public string? Aciklama { get; set; } // "21. yuzyil disiplinler arasi egitim yaklasimi.", "Nobel odullu Turk yazar.", null
    public string? HariciKayitNo { get; set; } // "LC-SH STEM", "VIAF:34456789", "TDK:MEG-001"
    
    //Navigation properties
    public ICollection<KatalogKonu> KatalogKonulari { get; set; } = new List<KatalogKonu>();
    public ICollection<KatalogKaydiYazar> IlgiliYazarKayitlari { get; set; } = new List<KatalogKaydiYazar>();
}
