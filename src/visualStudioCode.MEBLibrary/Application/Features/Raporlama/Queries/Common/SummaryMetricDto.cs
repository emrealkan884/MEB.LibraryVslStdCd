namespace Application.Features.Raporlama.Queries.Common;

public class SummaryMetricDto
{
    public SummaryMetricDto()
    {
    }

    public SummaryMetricDto(string key, string label, int value)
    {
        Key = key;
        Label = label;
        Value = value;
    }

    /// <summary>
    /// Makine tarafından okunabilir anahtar (ör. totalCatalogRecords).
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcıya gösterilecek etiket (lokalize).
    /// </summary>
    public string Label { get; set; } = string.Empty;

    public int Value { get; set; }
}
