using Domain.Enums;

namespace Domain.Entities;

public class YeniKatalogTalebi
{
    public int Id { get; set; } // 1001, 2050, 3095
    public int TalepEdenKutuphaneId { get; set; } // 205, 312, 478
    public string Baslik { get; set; } = string.Empty; // "STEM Proje Rehberi", "Kuşlar Atlası", "Matematik Olimpiyat Soruları"
    public string? AltBaslik { get; set; } // "Lise seviyesi etkinlikler", "Türkiye ve Avrupa türleri", null
    public string? YazarMetni { get; set; } // "Ayşe Yılmaz", "Milli Eğitim Bakanlığı", "John Doe & Jane Roe"
    public string? Isbn { get; set; } // "9786051234567", "9789754321098", null
    public string? Issn { get; set; } // "1303-6521", "2147-6484", null
    public string? MateryalTuru { get; set; } // "Kitap", "Sureli Yayin", "E-Kitap"
    public string? MateryalAltTuru { get; set; } // "Roman", "Aylik Bilim Dergisi", "Belgesel"
    public string? Dil { get; set; } // "Turkce", "Ingilizce", "Almanca"
    public string? Yayinevi { get; set; } // "MEB Yayınlari", "TUBITAK", "Oxford University Press"
    public string? YayinYeri { get; set; } // "Ankara", "Istanbul", "London"
    public int? YayinYili { get; set; } // 2024, 2019, null
    public string? Aciklama { get; set; } // "Fen lisesi müfredatı için gerekli.", "Robotik kulübü kaynak isteği.", null
    public TalepDurumu Durum { get; set; } = TalepDurumu.Beklemede; // TalepDurumu.Beklemede, TalepDurumu.Inceleniyor, TalepDurumu.Onaylandi
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow; // 2024-01-15 10:32, 2024-02-03 14:05, 2024-03-21 09:50
    public DateTime? SonGuncellemeTarihi { get; set; } // 2024-01-20 11:10, 2024-02-05 08:45, null
    public int? KatalogKaydiId { get; set; } // 120, 350, null

    public Kutuphane? TalepEdenKutuphane { get; set; }
    public KatalogKaydi? KatalogKaydi { get; set; }

    public void DurumuGuncelle(TalepDurumu yeniDurum)
    {
        Durum = yeniDurum;
        SonGuncellemeTarihi = DateTime.UtcNow;
    }

    public void KatalogKaydinaBagla(int katalogKaydiId)
    {
        KatalogKaydiId = katalogKaydiId;
        DurumuGuncelle(TalepDurumu.Onaylandi);
    }

    public void TalebiReddet(string? gerekce)
    {
        Aciklama = gerekce ?? Aciklama;
        DurumuGuncelle(TalepDurumu.Reddedildi);
    }
}
