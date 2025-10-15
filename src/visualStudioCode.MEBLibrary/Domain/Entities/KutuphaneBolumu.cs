namespace Domain.Entities;

public class KutuphaneBolumu
{
    public int Id { get; set; } // 10, 45, 88
    public int KutuphaneId { get; set; } // 205, 310, 512
    public string Ad { get; set; } = string.Empty; // "Fen Bilimleri", "Edebiyat", "Multimedya"
    public string? Aciklama { get; set; } // "STEM materyalleri için", "Okuma kulübü rafı", null

    public Kutuphane? Kutuphane { get; set; }
    public ICollection<Raf> Raflar { get; set; } = new List<Raf>();
    public ICollection<Materyal> Materyaller { get; set; } = new List<Materyal>();
}
