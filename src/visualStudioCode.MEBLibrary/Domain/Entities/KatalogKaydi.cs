using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class KatalogKaydi : Entity<Guid>
{
    public Guid KutuphaneId { get; set; } // 10 (Merkez), 205 (Yahya Turan Fen Lisesi), 510
    public string Baslik { get; set; } = string.Empty; // "Benim Adım Kırmızı", "STEM Eğitimi", "Kuşlar Ansiklopedisi"
    public string? AltBaslik { get; set; } // "Osmanlı Resim Sanatı Üzerine Bir Roman", "Fen Bilimleri İçin Proje Rehberi", "Türkiye Kuş Türleri"
    public string? Isbn { get; set; } // "9789754707083", "9786059852564", "9750800135"
    public string? Yayinevi { get; set; } // "İletişim Yayınları", "Milli Eğitim Yayınları", "TÜBİTAK Yayınları"
    public string? YayinYeri { get; set; } // "İstanbul", "Ankara", "New York"
    public int? YayinYili { get; set; } // 1998, 2015, 2023
    public int? SayfaSayisi { get; set; } // 445, 192, 864
    public string? Dil { get; set; } // "Türkçe", "İngilizce", "Almanca"
    public string? Dizi { get; set; } // "Karanlık Üçleme", "STEM Serisi", "Doğa Rehberi"
    public string? Baski { get; set; } // "2. Basım", "Gözden Geçirilmiş Basım", "İlk Basım"
    public string? Ozet { get; set; } // "16. yüzyılda geçen polisiye roman.", "STEM etkinlikleri için uygulamalı rehber.", "Türkiye'deki kuş türlerinin katalogu."
    public string? Notlar { get; set; } // "Kaynakça içerir.", "CD-ROM eklidir.", "Okuma kulübü önerisi."
    public string? KapakResmiYolu { get; set; } // "/covers/benim-adim-kirmizi.jpg", "https://cdn.meb.gov.tr/stem.jpg", "s3://library/covers/kuslar.png"
    public MateryalTuru MateryalTuru { get; set; } = MateryalTuru.Kitap; // MateryalTuru.Kitap, MateryalTuru.SureliYayin, MateryalTuru.Multimedya
    public string? MateryalAltTuru { get; set; } // "Roman", "Aylık Popüler Bilim", "Belgesel"
    public Guid? DeweySiniflamaId { get; set; } // 500, 813, 910
    public string? Marc21Verisi { get; set; } // "{ \"245\": {\"a\": \"Benim Adım Kırmızı\"} }", "{ \"100\": {\"a\": \"Pamuk, Orhan\"} }", "{ \"650\": {\"a\": \"STEM eğitimi\"} }"
    public bool RdaUyumlu { get; set; } // true, false, false
    public Guid? YeniKatalogTalebiId { get; set; } // 3201, 4502, null
    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow; // 2024-01-15 10:32, 2024-03-01 08:45, 2024-05-20 19:10
   
    
    //Navigation properties
    public Kutuphane? Kutuphane { get; set; }
    public DeweySiniflama? DeweySiniflama { get; set; }
    public YeniKatalogTalebi? YeniKatalogTalebi { get; set; }
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
    public ICollection<KatalogKaydiYazar> KatalogYazarlar { get; set; } = new List<KatalogKaydiYazar>();
    public ICollection<MateryalFormatDetay> FormatDetaylari { get; set; } = new List<MateryalFormatDetay>();
    public ICollection<KatalogKonu> Konular { get; set; } = new List<KatalogKonu>();
}
