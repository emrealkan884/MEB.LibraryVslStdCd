using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class MateryalEtiket : Entity<Guid>
{
    public MateryalEtiket()
    {
    }
    public MateryalEtiket(Guid id, Guid materyalId, string etiket, Materyal? materyal) : base(id)
    {
        MateryalId = materyalId;
        Etiket = etiket;
        Materyal = materyal;
    }
    public Guid MateryalId { get; set; } // 5001, 7820, 9105
    public string Etiket { get; set; } = string.Empty; // "Sınav Hazırlık", "Önerilen Kitaplar", "Robotik Kulübü"

    //Navigation properties
    public Materyal? Materyal { get; set; }
}
