namespace Domain.Entities;

public class Raf
{
    public int Id { get; set; } // Örnekler: 5, 18, 64
    public int KutuphaneBolumuId { get; set; } // Örnekler: 10, 45, 88
    public string Kod { get; set; } = string.Empty; // Örnekler: "FEN-A1", "EDEB-B2", "MULTI-DVD-1"
    public string? Aciklama { get; set; } // Örnekler: "9. sınıf fen kaynakları", "Okuma kulübü seçkisi", null

    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
}
