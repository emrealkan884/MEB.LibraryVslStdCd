using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class MateryalEtiket : Entity<Guid>
{
    public Guid MateryalId { get; set; } // 5001, 7820, 9105
    public string Etiket { get; set; } = string.Empty; // "Sınav Hazırlık", "Önerilen Kitaplar", "Robotik Kulübü"

    //Navigation properties
    public Materyal? Materyal { get; set; }
}
