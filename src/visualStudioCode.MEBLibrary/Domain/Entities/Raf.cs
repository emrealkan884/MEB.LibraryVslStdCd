using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Raf : Entity<Guid>
{
    public Guid KutuphaneBolumuId { get; set; } // 10, 45, 88
    public string Kod { get; set; } = string.Empty; // "FEN-A1", "EDEB-B2", "MULTI-DVD-1"
    public string? Aciklama { get; set; } // "9. sınıf fen kaynakları", "Okuma kulübü seçkisi", null

    //Navigation properties
    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
}
