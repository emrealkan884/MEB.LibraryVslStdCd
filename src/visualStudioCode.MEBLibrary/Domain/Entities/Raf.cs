namespace Domain.Entities;

public class Raf
{
    public int Id { get; set; } // 5, 18, 64
    public int KutuphaneBolumuId { get; set; } // 10, 45, 88
    public string Kod { get; set; } = string.Empty; // "FEN-A1", "EDEB-B2", "MULTI-DVD-1"
    public string? Aciklama { get; set; } // "9. sınıf fen kaynakları", "Okuma kulübü seçkisi", null

    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
}
