namespace Domain.Entities;

public class KatalogKaydi
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public string Baslik { get; set; } = string.Empty; // Baslik : AltBaslik
    public string? AltBaslik { get; set; } //Benim Adım Kırmızı : Osmanlı Resim Sanatı Üzerine Bir Roman
    public string? Isbn { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public string? Dil { get; set; }
    public string? Ozet { get; set; }
    public string? MateryalTuru { get; set; } //Kitap, dergi, video vb
    public string? MateryalAltTuru { get; set; } //Roman, belgesel, aylık popüler bilim vb.
    public int? KaynakTalepId { get; set; }
    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

    public Kutuphane? Kutuphane { get; set; }
    public YeniKatalogTalebi? KaynakTalep { get; set; }
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
    public ICollection<KatalogKaydiYazar> KatalogYazarlar { get; set; } = new List<KatalogKaydiYazar>();
}
