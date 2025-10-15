using Domain.Enums;

namespace Domain.Entities;

public class YeniKatalogTalebi
{
    public int Id { get; set; }
    public int TalepEdenKutuphaneId { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public string? AltBaslik { get; set; }
    public string? YazarMetni { get; set; }
    public string? Isbn { get; set; }
    public string? Issn { get; set; }
    public string? MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public string? Dil { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public string? Aciklama { get; set; }
    public TalepDurumu Durum { get; set; } = TalepDurumu.Beklemede;
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;
    public DateTime? SonGuncellemeTarihi { get; set; }
    public int? KatalogKaydiId { get; set; }

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
