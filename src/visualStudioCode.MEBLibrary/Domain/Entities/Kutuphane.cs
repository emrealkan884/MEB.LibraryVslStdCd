using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Kutuphane : Entity<Guid>
{
    public string Kod { get; set; } = string.Empty; // "MERKEZ-01", "ANK-CNK-05", "YTF-01"
    public string Ad { get; set; } = string.Empty; // "Merkez Kutuphanesi", "Cankaya Ilce Kutuphanesi", "Yahya Turan Fen Lisesi Kutuphanesi"
    public KutuphaneTipi Tip { get; set; } // KutuphaneTipi.Merkez, KutuphaneTipi.Okul, KutuphaneTipi.Okul
    public string Adres { get; set; } = string.Empty; // "Bakanliklar / Ankara", "Ataturk Bulvari 125 / Cankaya", "Fen Lisesi Kampusu / Izmir"
    public string? Telefon { get; set; } // "0312 123 45 67", "+90 232 765 43 21", null
    public string? EPosta { get; set; } // "merkez@meb.gov.tr", "kutuphane@fenlisesi.edu.tr", null
    public bool Aktif { get; set; } = true; // true, false, true

    //Navigation properties
    public ICollection<KatalogKaydi> KatalogKayitlari { get; set; } = new List<KatalogKaydi>();
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
    public ICollection<Etkinlik> Etkinlikler { get; set; } = new List<Etkinlik>();
    public ICollection<KutuphaneBolumu> Bolumler { get; set; } = new List<KutuphaneBolumu>();
    public ICollection<YeniKatalogTalebi> YeniKatalogTalepleri { get; set; } = new List<YeniKatalogTalebi>();
}
