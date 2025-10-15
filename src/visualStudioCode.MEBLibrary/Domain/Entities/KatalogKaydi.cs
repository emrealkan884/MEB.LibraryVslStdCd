using Domain.Enums;

namespace Domain.Entities;

public class KatalogKaydi
{
    public int Id { get; set; } // Örnekler: 1, 125, 4096
    public int KutuphaneId { get; set; } // Örnekler: 10 (Merkez), 205 (Yahya Turan Fen Lisesi), 510
    public string Baslik { get; set; } = string.Empty; // Örnekler: "Benim Adım Kırmızı", "STEM Eğitimi", "Kuşlar Ansiklopedisi"
    public string? AltBaslik { get; set; } // Örnekler: "Osmanlı Resim Sanatı Üzerine Bir Roman", "Fen Bilimleri İçin Proje Rehberi", "Türkiye Kuş Türleri"
    public string? Isbn { get; set; } // Örnekler: "9789754707083", "9786059852564", "9750800135"
    public string? Yayinevi { get; set; } // Örnekler: "İletişim Yayınları", "Milli Eğitim Yayınları", "TÜBİTAK Yayınları"
    public string? YayinYeri { get; set; } // Örnekler: "İstanbul", "Ankara", "New York"
    public int? YayinYili { get; set; } // Örnekler: 1998, 2015, 2023
    public int? SayfaSayisi { get; set; } // Örnekler: 445, 192, 864
    public string? Dil { get; set; } // Örnekler: "Türkçe", "İngilizce", "Almanca"
    public string? Dizi { get; set; } // Örnekler: "Karanlık Üçleme", "STEM Serisi", "Doğa Rehberi"
    public string? Baski { get; set; } // Örnekler: "2. Basım", "Gözden Geçirilmiş Basım", "İlk Basım"
    public string? Ozet { get; set; } // Örnekler: "16. yüzyılda geçen polisiye roman.", "STEM etkinlikleri için uygulamalı rehber.", "Türkiye'deki kuş türlerinin katalogu."
    public string? Notlar { get; set; } // Örnekler: "Kaynakça içerir.", "CD-ROM eklidir.", "Okuma kulübü önerisi."
    public string? KapakResmiYolu { get; set; } // Örnekler: "/covers/benim-adim-kirmizi.jpg", "https://cdn.meb.gov.tr/stem.jpg", "s3://library/covers/kuslar.png"
    public MateryalTuru MateryalTuru { get; set; } = MateryalTuru.Kitap; // Örnekler: MateryalTuru.Kitap, MateryalTuru.SureliYayin, MateryalTuru.Multimedya
    public string? MateryalAltTuru { get; set; } // Örnekler: "Roman", "Aylık Popüler Bilim", "Belgesel"
    public int? DeweySiniflamaId { get; set; } // Örnekler: 500, 813, 910
    public string? Marc21Verisi { get; set; } // Örnekler: "{ \"245\": {\"a\": \"Benim Adım Kırmızı\"} }", "{ \"100\": {\"a\": \"Pamuk, Orhan\"} }", "{ \"650\": {\"a\": \"STEM eğitimi\"} }"
    public bool RdaUyumlu { get; set; } // Örnekler: true, false, false
    public int? KaynakTalepId { get; set; } // Örnekler: 3201, 4502, null
    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow; // Örnekler: 2024-01-15 10:32, 2024-03-01 08:45, 2024-05-20 19:10

    public Kutuphane? Kutuphane { get; set; }
    public DeweySiniflama? DeweySiniflama { get; set; }
    public YeniKatalogTalebi? KaynakTalep { get; set; }
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
    public ICollection<KatalogKaydiYazar> KatalogYazarlar { get; set; } = new List<KatalogKaydiYazar>();
    public ICollection<MateryalFormatDetay> FormatDetaylari { get; set; } = new List<MateryalFormatDetay>();
    public ICollection<KatalogKonu> Konular { get; set; } = new List<KatalogKonu>();
}
