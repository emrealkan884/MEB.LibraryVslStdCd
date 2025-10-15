namespace Domain.Entities;

public class MateryalEtiket
{
    public int Id { get; set; }
    public int MateryalId { get; set; }
    public string Etiket { get; set; } = string.Empty;

    public Materyal? Materyal { get; set; }
}
