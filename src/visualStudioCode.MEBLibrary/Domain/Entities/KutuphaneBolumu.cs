namespace Domain.Entities;

public class KutuphaneBolumu
{
    public int Id { get; set; }
    public int KutuphaneId { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }

    public Kutuphane? Kutuphane { get; set; }
    public ICollection<Raf> Raflar { get; set; } = new List<Raf>();
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
}
