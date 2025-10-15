namespace Domain.Entities;

public class MateryalEtiket
{
    public int Id { get; set; } // Örnekler: 1, 22, 75
    public int MateryalId { get; set; } // Örnekler: 5001, 7820, 9105
    public string Etiket { get; set; } = string.Empty; // Örnekler: "Sınav Hazırlık", "Önerilen Kitaplar", "Robotik Kulübü"

    public Materyal? Materyal { get; set; }
}
