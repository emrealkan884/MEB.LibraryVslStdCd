using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class YeniKatalogTalebi : Entity<Guid>
{
    public YeniKatalogTalebi()
    {
    }

    public YeniKatalogTalebi(Guid id, Guid talepEdenKutuphaneId, string? altBaslik, string? yazarMetni, string? ısbn,
        string? ıssn, string? materyalTuru, string? materyalAltTuru, string? dil, string? yayinevi, string? yayinYeri,
        int? yayinYili, string? aciklama, string? redGerekcesi, TalepDurumu durum, DateTime? sonGuncellemeTarihi,
        Guid? katalogKaydiId, Kutuphane? talepEdenKutuphane, KatalogKaydi? katalogKaydi) : base(id)
    {
        TalepEdenKutuphaneId = talepEdenKutuphaneId;
        AltBaslik = altBaslik;
        YazarMetni = yazarMetni;
        Isbn = ısbn;
        Issn = ıssn;
        MateryalTuru = materyalTuru;
        MateryalAltTuru = materyalAltTuru;
        Dil = dil;
        Yayinevi = yayinevi;
        YayinYeri = yayinYeri;
        YayinYili = yayinYili;
        Aciklama = aciklama;
        RedGerekcesi = redGerekcesi;
        Durum = durum;
        SonGuncellemeTarihi = sonGuncellemeTarihi;
        KatalogKaydiId = katalogKaydiId;
        TalepEdenKutuphane = talepEdenKutuphane;
        KatalogKaydi = katalogKaydi;
    }

    public Guid
        TalepEdenKutuphaneId
    {
        get;
        set;
    } // 205 (Yahya Turan Fen Lisesi), 312 (Fen Bilimleri Anadolu Lisesi), 478 (Merkez Kytuphane)

    public string Baslik { get; set; } =
        string.Empty; // "STEM Proje Rehberi", "Kuslar Atlası", "Matematik Olimpiyat Soruları"

    public string? AltBaslik { get; set; } // "Lise seviyesi etkinlikler", "Türkiye ve Avrupa türleri", null
    public string? YazarMetni { get; set; } // "Ayşe Yılmaz", "Milli Eğitim Bakanlığı", "John Doe & Jane Roe"
    public string? Isbn { get; set; } // "9786051234567", "9789754321098", null
    public string? Issn { get; set; } // "1303-6521", "2147-6484", null
    public string? MateryalTuru { get; set; } // "Kitap", "Sureli Yayin", "E-Kitap"
    public string? MateryalAltTuru { get; set; } // "Roman", "Aylik Bilim Dergisi", "Belgesel"
    public string? Dil { get; set; } // "Turkce", "Ingilizce", "Almanca"
    public string? Yayinevi { get; set; } // "MEB Yayinlari", "TUBITAK", "Oxford University Press"
    public string? YayinYeri { get; set; } // "Ankara", "Istanbul", "London"
    public int? YayinYili { get; set; } // 2024, 2019, null
    public string? Aciklama { get; set; } // "Fen lisesi mufredati icin gerekli.", "Robotik kulubu kaynak istegi.", null
    public string? RedGerekcesi { get; set; } // "Talep edilen eser katalogda mevcut.", null, null

    public TalepDurumu Durum { get; set; } =
        TalepDurumu.Beklemede; // TalepDurumu.Beklemede, TalepDurumu.Inceleniyor, TalepDurumu.Onaylandi

    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow; // 2024-01-15 10:32, 2024-02-03 14:05, 2024-03-21 09:50
    public DateTime? SonGuncellemeTarihi { get; set; } // 2024-01-20 11:10, 2024-02-05 08:45, null

    public Guid?
        KatalogKaydiId
    {
        get;
        set;
    } // Guid.Parse("12000000-0000-0000-0000-000000000000"), null, Guid.Parse("35000000-0000-0000-0000-000000000000")

    public Kutuphane? TalepEdenKutuphane { get; set; }
    public KatalogKaydi? KatalogKaydi { get; set; }

    public void DurumuGuncelle(TalepDurumu yeniDurum)
    {
        Durum = yeniDurum;
        SonGuncellemeTarihi = DateTime.UtcNow;
    }

    public void TalebiIncelemeyeAl()
    {
        DurumuGuncelle(TalepDurumu.Inceleniyor);
    }

    public void KatalogKaydinaBagla(Guid katalogKaydiId)
    {
        KatalogKaydiId = katalogKaydiId;
        DurumuGuncelle(TalepDurumu.Onaylandi);
        RedGerekcesi = null;
    }

    public void TalebiReddet(string? gerekce)
    {
        RedGerekcesi = gerekce;
        DurumuGuncelle(TalepDurumu.Reddedildi);
    }

    public void TalepBilgileriniGuncelle(
        string baslik,
        string? altBaslik,
        string? yazarMetni,
        string? isbn,
        string? issn,
        string? materyalTuru,
        string? materyalAltTuru,
        string? dil,
        string? yayinevi,
        string? yayinYeri,
        int? yayinYili,
        string? aciklama
    )
    {
        Baslik = baslik;
        AltBaslik = altBaslik;
        YazarMetni = yazarMetni;
        Isbn = isbn;
        Issn = issn;
        MateryalTuru = materyalTuru;
        MateryalAltTuru = materyalAltTuru;
        Dil = dil;
        Yayinevi = yayinevi;
        YayinYeri = yayinYeri;
        YayinYili = yayinYili;
        Aciklama = aciklama;
        SonGuncellemeTarihi = DateTime.UtcNow;
    }
}