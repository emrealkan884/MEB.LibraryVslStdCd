namespace Domain.Entities;

public class Raf
{
    public int Id { get; set; }
    public int KutuphaneBolumuId { get; set; }
    public string Kod { get; set; } = string.Empty;
    public string? Aciklama { get; set; }

    public KutuphaneBolumu? Bolum { get; set; }
    public ICollection<Nusha> Nushalar { get; set; } = new List<Nusha>();
}
