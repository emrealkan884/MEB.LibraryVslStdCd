namespace Domain.Entities;

public class KatalogKaydi
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public string Baslik { get; set; } = string.Empty;
    public string? AltBaslik { get; set; }
    public string? Yazar { get; set; }
    public string? Isbn { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public string? Dil { get; set; }
    public string? Ozet { get; set; }
    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

    public Kutuphane? Kutuphane { get; set; }
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
}
