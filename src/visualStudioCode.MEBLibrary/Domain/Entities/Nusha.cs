using Domain.Enums;

namespace Domain.Entities;

public class Nusha
{
    public int Id { get; set; } // 1, 75, 420
    public int MateryalId { get; set; } // 5001, 7820, 9105
    public int? RafId { get; set; } // 12, 56, null
    public string Barkod { get; set; } = string.Empty; // "9789754707083-001", "STEM-2024-003", "DVD-0456"
    public NushaDurumu Durumu { get; set; } = NushaDurumu.Rafta; // NushaDurumu.Rafta, NushaDurumu.Oduncta, NushaDurumu.Bakimda
    public DateTime EklenmeTarihi { get; set; } = DateTime.UtcNow; // 2024-02-10 09:30, 2023-11-05 13:15, 2022-09-01 08:00

    public Materyal? Materyal { get; set; }
    public Raf? Raf { get; set; }
    public ICollection<OduncIslemi> OduncIslemleri { get; set; } = new List<OduncIslemi>();
}
