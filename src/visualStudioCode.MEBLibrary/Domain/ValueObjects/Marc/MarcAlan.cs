namespace Domain.ValueObjects.Marc;

public class MarcAlan
{
    public string Kod { get; set; } = string.Empty;  // Örn: "245"
    public string? Indikator1 { get; set; }          // Örn: "1"
    public string? Indikator2 { get; set; }          // Örn: "0"
    public List<MarcAltAlan> AltAlanlar { get; set; } = new();
}