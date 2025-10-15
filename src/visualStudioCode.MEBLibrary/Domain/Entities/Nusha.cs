using Domain.Enums;

namespace Domain.Entities;

public class Nusha
{
    public int Id { get; set; }
    public int MateryalId { get; set; }
    public int? RafId { get; set; }
    public string Barkod { get; set; } = string.Empty;
    public NushaDurumu Durumu { get; set; } = NushaDurumu.Rafta;
    public DateTime EklenmeTarihi { get; set; } = DateTime.UtcNow;

    public Materyal? Materyal { get; set; }
    public Raf? Raf { get; set; }
    public ICollection<OduncIslemi> OduncIslemleri { get; set; } = new List<OduncIslemi>();
}
